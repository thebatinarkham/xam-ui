namespace ABPRenamer
{
    public class Arguments
    {
        public readonly string filter = ".cs,.cshtml,.js,.ts,.csproj,.sln,.xml,.config,.DotSettings,.json,.xaml,.txt,.html,.gitignore,.ps1,.md,.plist";

        private string _oldCompanyName = "MyCompanyName";

        public string OldCompanyName
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(NewCompanyName))
                {
                    return _oldCompanyName;
                }
                return _oldCompanyName + ".";
            }
            set
            {
                _oldCompanyName = value;
            }
        }

        public string OldProjectName
        {
            get;
            set;
        } = "AbpZeroTemplate";


        public string NewCompanyName
        {
            get;
            set;
        }

        public string NewProjectName
        {
            get;
            set;
        }

        public string OldAreaName
        {
            get;
            set;
        } = "AppAreaName";


        public string NewAreaName
        {
            get;
            set;
        } = "App";


        public string RootDir
        {
            get;
            set;
        }
    }
}
