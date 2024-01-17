using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System.Windows;

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

            node = node.GetFirstChild();

            CreateTreeFromSW(node, "");

           // var treeInstance = sWobject.InfoAboutPart();



        }
            public void CreateTreeFromSW(TreeControlItem node, string parentNum)
            {
                int nodeType;
                ModelDoc2 swModel;
                TreeControlItem childNode;
                Component2 swComp;
                int drawingNum = 1;
                string evaluatedParentNum;
                string parentNumOld;
                string finalDrawingNum = drawingNum.ToString();

                while (node != null)
                {
                    nodeType = node.ObjectType;
                    if (nodeType == (int)swTreeControlItemType_e.swFeatureManagerItem_Component)
                    {
                        swComp = (Component2)node.Object;
                        swModel = swComp.GetModelDoc2();
                        if (swModel.GetType() == (int)swDocumentTypes_e.swDocASSEMBLY)
                        {
                            childNode = node.GetFirstChild();
                            parentNumOld = parentNum;
                            parentNum = $"{parentNum}.{drawingNum}";
                            //add position in a tree view 
                            swTreeView.Items.Add(parentNumOld);
                            CreateTreeFromSW(childNode, parentNum);
                            parentNum = parentNumOld;
                        }
                        else
                        {

                            evaluatedParentNum = $"{parentNum}.{drawingNum}";
                        //add position in a tree view
                            swTreeView.Items.Add(evaluatedParentNum);
                    }
                        drawingNum++;
                        
                    }
                    node = node.GetNext();
                }
            }
        
    }
}
