using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Feng.Windows.Forms;

namespace Feng
{
    /// <summary>
    /// 
    /// </summary>
    public static class ProcessSelect
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dm"></param>
        /// <param name="windowName"></param>
        /// <param name="presetValues"></param>
        /// <returns></returns>
        public static ArchiveCheckForm Execute(IDisplayManager dm, string windowName, Dictionary<string, object> presetValues)
        {
            ArchiveCheckForm checkForm = new GeneratedArchiveCheckForm(ADInfoBll.Instance.GetWindowInfo(windowName));

            return Execute(dm, checkForm, presetValues);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dm"></param>
        /// <param name="checkForm"></param>
        /// <param name="presetValues"></param>
        /// <returns></returns>
        public static ArchiveCheckForm Execute(IDisplayManager dm, ArchiveCheckForm checkForm, Dictionary<string, object> presetValues)
        {
            if (presetValues != null)
            {
                foreach (KeyValuePair<string, object> kvp in presetValues)
                {
                    if (kvp.Value == null)
                    {
                        MessageForm.ShowError("请先填写" + kvp.Key + "!");
                        return null;
                    }
                }

                ISearchManager smc = checkForm.DisplayManager.SearchManager;
                if (smc != null)
                {
                    foreach (KeyValuePair<string, object> kvp in presetValues)
                    {
                        smc.SearchControls[kvp.Key].SelectedDataValues = new System.Collections.ArrayList { kvp.Value };
                    }
                }
            }

            DialogResult ret = checkForm.ShowDialog();
            if (ret == DialogResult.OK)
            {
                if (presetValues != null)
                {
                    foreach (KeyValuePair<string, object> kvp in presetValues)
                    {
                        dm.DataControls[kvp.Key].ReadOnly = true;

                        // save controlValue to entity
                        EntityScript.SetPropertyValue(dm.CurrentItem, kvp.Key,
                            dm.DataControls[kvp.Key].SelectedDataValue);
                    }
                }

                //IControlManager detailCm = ((ArchiveDetailFormAutoWithDetailGrid)detailForm).DetailGrid.ControlManager;
                return checkForm;
            }

            return null;
        }
    }
}
