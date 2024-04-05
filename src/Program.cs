using AutomatizationVersionUpdate.AvroService;
using AutomatizationVersionUpdate.CreateClassService;

//Generate Class from already existing one
GenerateClassDynamically test = new GenerateClassDynamically();
ClassGenerateSettings settings = new ClassGenerateSettings();
settings.SaveFileDirectory = "C:\\Users\\steli\\Desktop\\AutoAvroGen\\AvroSchemaGenerator\\AvroSchemaGenerator\\src\\test\\";
//settings.IsSeparateClasses = false;
settings.IsSchemaInclude = true;
//settings.SaveFileDirectory = "C:\\Users\\stelian.r\\Desktop\\testPath";

test.GenerateClassFromExistingClass<TestClass1>("test", "test.v1", settings);

//Generate Avro schema from class
GenerateAvroSchemaDynamically avroSchema = new GenerateAvroSchemaDynamically();
//avroSchema.GenerateAvroSchemaFromJson(json);

Thread.Sleep(200);
var schema = avroSchema.GenerateAvroSchemaFromClass<TestClass1>();
avroSchema.WriteFile(schema, "test", "avsc", "test.v1");


//Used for testing => Create DTO from existing class
public class TestClass1
{
    public int[] IntArray { get; set; }

    public AutomatizationVersionUpdate.TestClass2 TestClass2 { get; set; }
    public List<AutomatizationVersionUpdate.TestClass2> TestClass2List { get; set; }
    //public Dictionary<TestForProp, TestForProp> ObjectToObject { get; set; }
    //public Dictionary<string, TestForProp> Lines { get; set; }
    //public Dictionary<bool, string> MyProperty { get; set; }
    //public Dictionary<string, IMGMessage> ImgDict { get; set; }
    //public IMGMessage Surprice { get; set; }
}


