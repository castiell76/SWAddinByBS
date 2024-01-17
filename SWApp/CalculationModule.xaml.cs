using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using static PdfSharp.Snippets.Font.NewFontResolver;

namespace SWApp
{
    /// <summary>
    /// Interaction logic for CalculationModule.xaml
    /// </summary>
    public partial class CalculationModule : Window
    {
        public SWObject sWobject = new SWObject();
        public CalculationModule()
        {
            InitializeComponent();

    }
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            SWTreeInit();
        }



        public void SWTreeInit()
        {
            SldWorks swApp = (SldWorks)Marshal2.GetActiveObject("SldWorks.Application");
            ModelDoc2 swModel = swApp.ActiveDoc;
            FeatureManager swFeatMgr = swModel.FeatureManager;
            TreeControlItem node = swFeatMgr.GetFeatureTreeRootItem2((int)swFeatMgrPane_e.swFeatMgrPaneBottom);
            string assemblyName = System.IO.Path.GetFileNameWithoutExtension(swModel.GetPathName());

            //add main node assembly
            SWTreeNode mainAssembly = new SWTreeNode() { Name = assemblyName };
            //add other nodes
            SWTreeNode treeNode = new SWTreeNode() { Name = assemblyName };

            node = node.GetFirstChild();
            treeNode=  CreateTreeFromSW(node, "", treeNode);
            mainAssembly.Items.Add(treeNode);

            //assign nodes to the treeView
            swTreeView.ItemsSource = mainAssembly.Items ;

        }
            public SWTreeNode CreateTreeFromSW(TreeControlItem node, string parentNum, SWTreeNode swTreeNodes)
            {
                int nodeType;
                ModelDoc2 swModel;
                TreeControlItem childNode;
                Component2 swComp;
                int drawingNum = 1;
                string evaluatedParentNum;
                string parentNumOld;
                string finalDrawingNum = drawingNum.ToString();
                string name;
                

            while (node != null)
                {
                    nodeType = node.ObjectType;
                    if (nodeType == (int)swTreeControlItemType_e.swFeatureManagerItem_Component)
                    {
                        swComp = (Component2)node.Object;
                        swModel = swComp.GetModelDoc2();
                        name = System.IO.Path.GetFileNameWithoutExtension(swModel.GetPathName());
                    if (swModel.GetType() == (int)swDocumentTypes_e.swDocASSEMBLY)
                        {
                            childNode = node.GetFirstChild();
                            
                            parentNumOld = parentNum;
                            parentNum = $"{parentNum}.{drawingNum}";
                        //add position in a tree level 
                            SWTreeNode newTreeLevel = new SWTreeNode() { Name = name };
                            
                            CreateTreeFromSW(childNode, parentNum, newTreeLevel);
                        swTreeNodes.Items.Add(newTreeLevel);
                        parentNum = parentNumOld;
                        }
                        else
                        {
                            evaluatedParentNum = $"{parentNum}.{drawingNum}";
                        //add position in a tree view
                        swTreeNodes.Items.Add(new SWTreeNode() { Name = name });
                        }
                        drawingNum++;
                        
                    }
                    node = node.GetNext();
                }

            return swTreeNodes;
            }
        
    }
}
