using DAO;
using DevExpress.Utils;
using DevExpress.Utils.Svg;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace Tooling_WH_Sytems
{
    class TablePermission
    {
        private static TablePermission _defaultInstance;
        public static TablePermission Instance
        {
            get
            {
                if (_defaultInstance == null)
                {
                    _defaultInstance = new TablePermission();
                }
                return _defaultInstance;
            }
            set => _defaultInstance = value;
        }

        public TablePermission()
        {

        }
        public DataTable CreatedTablePermission()
        {
            DataTable table_ribbon = new DataTable();
           
            table_ribbon.Columns.Add("id", typeof(string));
            table_ribbon.Columns.Add("parentid", typeof(string));
            table_ribbon.Columns.Add("caption", typeof(string));
            table_ribbon.Columns.Add("ispage", typeof(int));
            table_ribbon.Columns.Add("isgrouppage", typeof(int));
            table_ribbon.Columns.Add("User_Right", typeof(string));
            return table_ribbon;
        }
        public Tuple<DataTable, ImageCollection> GetAllTableMenu(RibbonControl ribbonControl,int idUser)
        {
            var imageCollection = new ImageCollection();
            DataTable table_ribbon = CreatedTablePermission();
            ArrayList visiblePages = ribbonControl.TotalPageCategory.GetVisiblePages();

            foreach (RibbonPage page in visiblePages)
            {
                string user_Right = "Cấm quyền";
                var page_name = page.Name;
                var page_caption = page.Text;
                imageCollection.AddImage(null, page_name);
                if (FunctionDetailDAO.Instance.TestFuncDetail(page_name) == -1)
                {
                    FunctionDetailDAO.Instance.InsertFuctionDetail(page_name, "0", page_caption);
                    user_Right = UserRightDAO.Instance.NameUserRight(idUser, page_name);
                }
                else
                {
                    user_Right = UserRightDAO.Instance.NameUserRight(idUser, page_name);
                }
                table_ribbon.Rows.Add(page_name, "0", page_caption, 1, 0, user_Right);
                foreach (RibbonPageGroup group in page.Groups)
                {
                    
                    foreach (BarItemLink item in group.ItemLinks)
                    {
                      
                        var item_caption = item.Caption;
                        var item_name = item.Item.Name;
                        if (item_name.Equals("btnUserGroup") || item_name.Equals("btnSkin"))
                        {
                            break;
                        }
                
                            if (FunctionDetailDAO.Instance.TestFuncDetail(item_name) == -1)
                            {
                                FunctionDetailDAO.Instance.InsertFuctionDetail(item_name, page_name, item_caption);
                                user_Right = UserRightDAO.Instance.NameUserRight(idUser, item_name);

                            }
                            else
                            {
                                user_Right = UserRightDAO.Instance.NameUserRight(idUser, item_name);
                            }
                            table_ribbon.Rows.Add(item_name, page_name, item_caption, 0, 0, user_Right);
                        
                    }
                }
            }
            return Tuple.Create(table_ribbon, imageCollection);
        }
    }
}
