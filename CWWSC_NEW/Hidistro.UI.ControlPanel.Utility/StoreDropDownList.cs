using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Commodities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Hidistro.UI.ControlPanel.Utility
{
    public class StoreDropDownList:DropDownList
    {
        private bool allowNull;
        private string nullToDisplay = "";

        public override void DataBind()
        {
            this.Items.Clear();
            if (this.AllowNull)
            {
                base.Items.Add(new ListItem(this.NullToDisplay, string.Empty));
            }
            foreach (StoreInfo info in StoreHelper.GetStore())
            {
                this.Items.Add(new ListItem(Globals.HtmlDecode(info.storeName), info.accountALLHere.ToString()));
            }
        }

        public bool AllowNull
        {
            get
            {
                return this.allowNull;
            }
            set
            {
                this.allowNull = value;
            }
        }

        public string NullToDisplay
        {
            get
            {
                return this.nullToDisplay;
            }
            set
            {
                this.nullToDisplay = value;
            }
        }

        new public string SelectedValue
        {
            get
            {
                if (string.IsNullOrEmpty(base.SelectedValue))
                {
                    return null;
                }
                return  base.SelectedValue;
            }
            set
            {
                if (!string.IsNullOrEmpty( value))
                {
                    base.SelectedIndex = base.Items.IndexOf(base.Items.FindByValue(value));
                }
            }
        }
    }
}
