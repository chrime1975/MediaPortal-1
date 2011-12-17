#region Copyright (C) 2005-2011 Team MediaPortal

// Copyright (C) 2005-2011 Team MediaPortal
// http://www.team-mediaportal.com
// 
// MediaPortal is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 2 of the License, or
// (at your option) any later version.
// 
// MediaPortal is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with MediaPortal. If not, see <http://www.gnu.org/licenses/>.

#endregion

using System;
using System.Windows.Forms;

namespace Mediaportal.TV.Server.SetupTV.Dialogs
{
  public partial class FormDVBIPTuningDetail : SetupControls.FormTuningDetailCommon
  {
    public FormDVBIPTuningDetail()
    {
      InitializeComponent();
    }

    private void FormDVBIPTuningDetail_Load(object sender, EventArgs e)
    {
      if (TuningDetail != null)
      {
        textBoxDVBIPChannel.Text = TuningDetail.channelNumber.ToString();
        textBoxDVBIPUrl.Text = TuningDetail.url;
        textBoxDVBIPNetworkId.Text = TuningDetail.networkId.ToString();
        textBoxDVBIPTransportId.Text = TuningDetail.transportId.ToString();
        textBoxDVBIPServiceId.Text = TuningDetail.serviceId.ToString();
        textBoxDVBIPPmtPid.Text = TuningDetail.pmtPid.ToString();
        textBoxDVBIPProvider.Text = TuningDetail.provider;
        checkBoxDVBIPfta.Checked = TuningDetail.freeToAir;
      }
      else
      {
        textBoxDVBIPChannel.Text = "";
        textBoxDVBIPUrl.Text = "";
        textBoxDVBIPNetworkId.Text = "";
        textBoxDVBIPTransportId.Text = "";
        textBoxDVBIPServiceId.Text = "";
        textBoxDVBIPPmtPid.Text = "";
        textBoxDVBIPProvider.Text = "";
        checkBoxDVBIPfta.Checked = false;
      }
    }

    private void mpButtonOk_Click(object sender, EventArgs e)
    {
      if (ValidateInput())
      {
        if (TuningDetail == null)
        {
          TuningDetail = CreateInitialTuningDetail();
        }
        UpdateTuningDetail();
        DialogResult = DialogResult.OK;
        Close();
      }
    }

    private void UpdateTuningDetail()
    {
      TuningDetail.channelType = 7;
      TuningDetail.channelNumber = Int32.Parse(textBoxDVBIPChannel.Text);
      TuningDetail.url = textBoxDVBIPUrl.Text;
      TuningDetail.networkId = Int32.Parse(textBoxDVBIPNetworkId.Text);
      TuningDetail.transportId = Int32.Parse(textBoxDVBIPTransportId.Text);
      TuningDetail.serviceId = Int32.Parse(textBoxDVBIPServiceId.Text);
      TuningDetail.pmtPid = Int32.Parse(textBoxDVBIPPmtPid.Text);
      TuningDetail.provider = textBoxDVBIPProvider.Text;
      TuningDetail.freeToAir = checkBoxDVBIPfta.Checked;
    }

    private bool ValidateInput()
    {
      int lcn, onid, tsid, sid, pmt;
      if (textBoxDVBIPChannel.Text.Length == 0)
      {
        MessageBox.Show(this, "Please enter a channel number!", "Incorrect input");
        return false;
      }
      if (!Int32.TryParse(textBoxDVBIPChannel.Text, out lcn))
      {
        MessageBox.Show(this, "Please enter a valid channel number!", "Incorrect input");
        return false;
      }
      if (textBoxDVBIPUrl.Text.Length == 0)
      {
        MessageBox.Show(this, "Please enter a valid URL!", "Incorrect input");
        return false;
      }
      if (!Int32.TryParse(textBoxDVBIPNetworkId.Text, out onid))
      {
        MessageBox.Show(this, "Please enter a valid network ID!", "Incorrect input");
        return false;
      }
      if (!Int32.TryParse(textBoxDVBIPTransportId.Text, out tsid))
      {
        MessageBox.Show(this, "Please enter a valid transport ID!", "Incorrect input");
        return false;
      }
      if (!Int32.TryParse(textBoxDVBIPServiceId.Text, out sid))
      {
        MessageBox.Show(this, "Please enter a valid service ID!", "Incorrect input");
        return false;
      }
      if (onid < 0 || tsid < 0 || sid < 0)
      {
        MessageBox.Show(this, "Please enter valid network, transport and service IDs!", "Incorrect input");
        return false;
      }
      if (!Int32.TryParse(textBoxDVBIPPmtPid.Text, out pmt))
      {
        MessageBox.Show(this, "Please enter a valid PMT PID!", "Incorrect input");
        return false;
      }
      if (pmt < 0)
      {
        MessageBox.Show(this, "Please enter a valid PMT PID!", "Incorrect input");
        return false;
      }
      return true;
    }
  }
}