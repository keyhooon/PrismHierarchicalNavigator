namespace CompositeContentNavigator
{
    public class ModuleConfig
    {

        public static string SectionName = "ContentNavigator";

        public ModuleConfig()
        {

            HasRoot = false;
            RootDisplay = "";
            ContentRegionName  = "ContentRegion";
            ContentMapRegionName  = "ContentMapRegion";
            HeaderRegionName = "HeaderRegion";
            ToolbarRegionName = "ToolbarRegion";

    }

        public bool HasRoot { get; set; } 

        public string RootDisplay { get; set; } 

        public string ContentRegionName { get; set; } 

        public string ContentMapRegionName { get; set; } 

        public string HeaderRegionName { get; set; }

        public string ToolbarRegionName { get; set; }

    }
}
