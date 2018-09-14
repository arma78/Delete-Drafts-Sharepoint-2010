using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;
using Microsoft.Office.DocumentManagement.DocumentSets;
using System.Collections.Generic;
using System.Threading;

namespace DeleteDrafts_EventReceiver.DeleteDrafts_EventReceiver
{
    /// <summary>
    /// List Item Events
    /// </summary>
    public class DeleteDrafts_EventReceiver : SPItemEventReceiver
    {
        /// <summary>
        /// An item was added.
        /// </summary>
        public override void ItemAdded(SPItemEventProperties properties)
        {
            Thread.Sleep(60000);            
            
            base.EventFiringEnabled = false;
            SPSecurity.RunWithElevatedPrivileges(delegate()
              {
                  using (SPSite site = new SPSite(properties.WebUrl))
                  {
                      using (SPWeb web = site.OpenWeb())
                      {
                          SPList oList = properties.Web.Lists["ReviewDocs"];
                          string itemId = properties.ListItem["ID"].ToString();
                          SPListItem sourceItem = oList.GetItemById(Convert.ToInt32(itemId));
                                                
                                                     
                              SPView oView = oList.Views["Delete"]; 
                              SPQuery query = new SPQuery(oView); 
                              SPListItemCollection listItemsCollection = oList.GetItems(query);

                              for (int i = listItemsCollection.Count - 1; i >= 0; i--)
			                                    {                                                   
                                                    SPListItem file = listItemsCollection[i];                              
                                                               file.File.Delete();
			                                    }                                                           
                      }

                  }
              });
           
            base.EventFiringEnabled = true;
        }
    }
}

    

