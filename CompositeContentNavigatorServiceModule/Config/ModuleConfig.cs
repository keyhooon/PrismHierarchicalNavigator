namespace CompositeContentNavigatorServiceModule.Config
{
    public class ModuleConfig
    {
        public ModuleConfig()
        {

            HasRoot = false;
            RootDisplay = "";
            ContentRegionName  = "ContentRegion";
            ToolbarRegionName  = "ToolBarRegion";
            HeaderRegionName = "HeaderRegion";

        }

        public static string SectionName = "ContentNavigator";
        public bool HasRoot { get; set; } 

        public string RootDisplay { get; set; } 

        public string ContentRegionName { get; set; } 

        public string ToolbarRegionName { get; set; } 

        public string HeaderRegionName { get; set; } 

    }
}
