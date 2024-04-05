namespace AutomatizationVersionUpdate.CreateClassService
{
    using System.Reflection;

    public class ClassLayout
    {
        public string? Name { get; set; }
        public string? FullName { get; set; }
        public string? Type { get; set; }

        public string? Namespace { get; set; }

        public List<PropertyInfo> PropertyInfos { get; set; }
        /// <summary>
        /// To be able to skip property create with same class name
        /// It is used when we have prop of type List, Array, Dictionary
        /// </summary>
        public bool CreateAsProperty { get; set; }
        public Dictionary<string, dynamic>? Properties { get; set; }

        public List<ClassLayout> ClassCollection { get; set; }

        public CustomType CreateType(Type type)
        {
            return new CustomType(type, this);
        }
    }
}
