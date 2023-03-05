namespace AutomatizationVersionUpdate.CreateClassService
{
    using AutomatizationVersionUpdate.CreateClassService.Enums;

    public class ClassGenerateSettings
    {
        //public bool IsSeparateClasses {get; set;}
        public bool IsSchemaInclude { get; set; }
        public EnumConvert EnumConvert { get; set; }

        public string SaveFileDirectory { get; set; }

        //public DateTimeConvert DateTimeConvert { get; set; }
        //public DateTimeOffSetConvert DateTimeOffSetConvert { get; set; }
    }
}

