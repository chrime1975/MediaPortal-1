#region Copyright (C) 2008 Team MediaPortal

/*
    Copyright (C) 2008 Team MediaPortal
    http://www.team-mediaportal.com
 
    This file is part of MediaPortal II

    MediaPortal II is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    MediaPortal II is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with MediaPortal II.  If not, see <http://www.gnu.org/licenses/>.
*/

#endregion

#region usings
using System;
using System.Collections.Generic;
using System.Text;

using MediaPortal.Core;
using MediaPortal.Core.Logging;
using MediaPortal.Core.Settings;
using MediaPortal.Core.Messaging;
using MediaPortal.Core.Threading;
using MediaPortal.Core.TaskScheduler;
#endregion

namespace MediaPortal.Services.TaskScheduler
{
  /// <summary>
  /// Default implementation of the TaskScheduler. See the <see cref="ITaskScheduler"/> description for details.
  /// </summary>
  public class TaskScheduler : ITaskScheduler
  {
    #region variables
    /// <summary>
    /// The "taskscheduler" message queue.
    /// </summary>
    private IQueue _queue;
    /// <summary>
    /// The persistent settings used by the task scheduler
    /// </summary>
    private TaskSchedulerSettings _settings;
    /// <summary>
    /// interval-based work object to periodically check for due tasks.
    /// </summary>
    private IntervalWork _work;
    /// <summary>
    /// mutex object to serialize access to the registered tasks and next TaskID
    /// </summary>
    private object _taskMutex = new object();
    #endregion

    #region Constructor
    public TaskScheduler()
    {
      _queue = ServiceScope.Get<IMessageBroker>().Get("taskscheduler");
      _settings = new TaskSchedulerSettings();
      ServiceScope.Get<ISettingsManager>().Load(_settings);
      SaveChanges(false);

      DoStartup();
      _work = new IntervalWork(new DoWorkHandler(this.DoWork), new TimeSpan(0, 0, 20));
      ServiceScope.Get<IThreadPool>().AddIntervalWork(_work, false);
    }
    #endregion

    #region Destructor
    ~TaskScheduler()
    {
      _work.Cancel();
      ServiceScope.Get<IThreadPool>().RemoveIntervalWork(_work);
      ServiceScope.Remove<ITaskScheduler>();
      ServiceScope.Get<ISettingsManager>().Save(_settings);
    }
    #endregion

    #region Private methods
    /// <summary>
    /// Triggers the registered tasks which are registered to fire at startup.
    /// Called from the Constructor.
    /// </summary>
    private void DoStartup()
    {
      DateTime now = DateTime.Now;
      // only use minute precision
      now = now.AddSeconds(-now.Second);
      lock (_taskMutex)
      {
        foreach (Task task in _settings.TaskCollection.Tasks)
        {
          if (task.Occurrence == Occurrence.EveryStartUp)
          {
            if (task.IsExpired(now))
            {
              ExpireTask(task);
            }
            else
            {
              ProcessTask(task);
            }
          }
        }
      }
    }

    /// <summary>
    /// Registed as delegate with the <see cref="IntervalWork"/> item to periodically check for due tasks
    /// </summary>
    private void DoWork()
    {
      bool needSort = false;
      bool saveChanges = false;
      DateTime now = DateTime.Now;
      // only use minute precision
      now = now.AddSeconds(-now.Second);
      // Exclusively get lock for task collection access
      lock (_taskMutex)
      {
        // enumerate through all tasks
        foreach (Task task in _settings.TaskCollection.Tasks)
        {
          // only process non-repeat tasks
          if (task.Occurrence == Occurrence.Once || task.Occurrence == Occurrence.Repeat)
          {
            ServiceScope.Get<ILogger>().Debug("TaskScheduler: ProcessTask: {0}", task.ToString());
            // Process task if schedule is due
            if (task.IsDue(now))
            {
              ProcessTask(task);
              needSort = true;
              saveChanges = true;
            }
            else
            {
              // Force process task if schedule is due late and ForceRun is set
              if (task.NextRun < now)
              {
                if (task.ForceRun)
                {
                  ProcessTask(task);
                  needSort = true;
                  saveChanges = true;
                }
              }
            }
          }
          // Expire all type of tasks if they are expired
          if (task.IsExpired(now))
          {
            ExpireTask(task);
            saveChanges = true;
          }
        }
        if (saveChanges)
        {
          SaveChanges(needSort);
        }
      }
    }

    /// <summary>
    /// Handles expired tasks: sends out a message that a particular task is expired, removes the
    /// task from the registered task collection and sends out a message that the task is deleted.
    /// This method gets called from the DoWork() method.
    /// </summary>
    /// <param name="task">Task to expire</param>
    private void ExpireTask(Task task)
    {
      SendMessage(new TaskMessage(task.Clone() as Task, TaskMessageType.EXPIRED));
      _settings.TaskCollection.Remove(task);
      SendMessage(new TaskMessage(task.Clone() as Task, TaskMessageType.DELETED));
      SaveChanges(false);
    }

    /// <summary>
    /// Handles processing of due tasks. Sends out a message that a task is due, updates the tasks
    /// properties and removes the task if it was scheduled to run only once. This method gets called
    /// from the DoWork() method.
    /// </summary>
    /// <param name="task">Task to process</param>
    private void ProcessTask(Task task)
    {
      SendMessage(new TaskMessage(task.Clone() as Task, TaskMessageType.DUE));
      task.LastRun = task.NextRun;
      if (task.Occurrence == Occurrence.Once)
      {
        _settings.TaskCollection.Remove(task);
        SendMessage(new TaskMessage(task.Clone() as Task, TaskMessageType.DELETED));
      }
    }

    /// <summary>
    /// Handles broadcasting of messages from the task scheduler. Embeds the given <see cref="TaskMessage"/>
    /// into a generic message.
    /// </summary>
    /// <param name="message">message to send</param>
    private void SendMessage(TaskMessage message)
    {
      // create message
      MPMessage msg = new MPMessage();
      msg.MetaData["taskmessage"] = message;
      // asynchronously send message through queue
      ServiceScope.Get<IThreadPool>().Add(new Work(new DoWorkHandler(delegate() { _queue.Send(msg); })));
    }

    /// <summary>
    /// Saves any changes to the task configuration.
    /// </summary>
    /// <param name="needSort">specifies whether or not the task collection needs to be resorted</param>
    private void SaveChanges(bool needSort)
    {
      if (needSort)
        _settings.TaskCollection.Sort();
      ServiceScope.Get<ISettingsManager>().Save(_settings);
    }
    #endregion

    #region ITaskScheduler implementation
    public int AddTask(Task newTask)
    {
      lock (_taskMutex)
      {
        newTask.ID = _settings.GetNextTaskID();
        _settings.TaskCollection.Add(newTask);
        SaveChanges(false);
      }
      return newTask.ID;
    }

    public void UpdateTask(int taskId, Task updatedTask)
    {
      lock (_taskMutex)
      {
        updatedTask.ID = taskId;
        _settings.TaskCollection.Replace(taskId, updatedTask);
        SaveChanges(false);
        SendMessage(new TaskMessage(updatedTask, TaskMessageType.CHANGED));
      }
    }
    public void RemoveTask(int taskId)
    {
      lock (_taskMutex)
      {
        Task task = null;
        foreach (Task t in _settings.TaskCollection.Tasks)
        {
          if (t.ID == taskId)
          {
            task = t;
          }
        }
        if (task != null)
        {
          _settings.TaskCollection.Remove(task);
          SaveChanges(false);
          SendMessage(new TaskMessage(task, TaskMessageType.DELETED));
        }
      }
    }
    public Task GetTask(int taskId)
    {
      List<Task> allTasks;
      lock (_taskMutex)
      {
        allTasks = _settings.TaskCollection.Clone();
      }
      foreach (Task task in allTasks)
      {
        if (task.ID == taskId)
        {
          return task;
        }
      }
      return null;
    }
    public List<Task> GetTasks(string ownerId)
    {
      List<Task> allTasks;
      List<Task> tasks = new List<Task>();
      lock (_taskMutex)
      {
        allTasks = _settings.TaskCollection.Clone();
      }
      foreach (Task task in allTasks)
      {
        if (task.Owner.Equals(ownerId))
        {
          tasks.Add(task);
        }
      }
      return tasks;
    }
    #endregion
  }
}
