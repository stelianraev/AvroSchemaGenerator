using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutomatizationVersionUpdate.AvroService.Models;
using Avro;
using Newtonsoft.Json;

namespace AutomatizationVersionUpdate.CreateClassService
{
    public class CreateClassRefactoring
    {
        //public ClassLayout GetClassPropertyCollectionUrban(Type type, ClassLayout classLayout, List<ClassLayout> classDublicates, ClassGenerateSettings settings = null)
        //{
        //    var dictionary = new Dictionary<string, dynamic>();
        //    classLayout.Properties = dictionary;
        //    classLayout.ClassCollection = new List<ClassLayout>();  

        //    PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);

        //    GetClassPropertyCollection(properties, classLayout, classDublicates, settings);

        //    var avro = GetAvroPropertyCollectionUrban(type, new AvroSchema(), new List<AvroSchema>(), settings);

        //    if (settings != null && settings.IsSchemaInclude)
        //    {
        //        var jsonConv = JsonConvert.SerializeObject(avro);
        //        var res = JsonConvert.ToString(jsonConv);
        //        classLayout.Properties["_Schema"] = res;

        //        Console.WriteLine(JsonConvert.ToString(classLayout.Properties["_Schema"]));
        //    }

        //    return classLayout;
        //}

        //private AvroSchema GetAvroPropertyCollectionUrban(Type type, AvroSchema schema, List<AvroSchema> classDublicates, ClassGenerateSettings settings = null)
        //{
        //    string typeName = type.Name.TrimEnd(new char[] { '[', ']' });
        //    //string propFullName = type.FullName.TrimEnd(new char[] { '[', ']' });           

        //    schema.Type = "record";
        //    schema.Name = typeName;
        //    schema.Namespace = type.Namespace;

        //    schema.Fields = new List<AvroField>();

        //    PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);

        //    return GetAvroSchema(properties, schema, classDublicates);
        //}

        //private AvroSchema GetAvroSchema(PropertyInfo[] properties, AvroSchema schema, List<AvroSchema> dublicates)
        //{
        //    foreach (var prop in properties)
        //    {
        //        var field = new AvroField();
        //        field.Name = prop.Name;

        //        schema.Fields.Add(field);

        //        if (prop.PropertyType == typeof(int))
        //        {
        //            field.Type = "int";
        //            field.Default = 0;

        //            continue;
        //        }
        //        if (prop.PropertyType == typeof(long))
        //        {
        //            field.Type = "long";
        //            field.Default = 0;
        //        }
        //        else if (prop.PropertyType == typeof(string))
        //        {
        //            field.Type = new string[]
        //            {
        //                "null",
        //                "string"
        //            };

        //            field.Default = null;

        //            continue;
        //        }
        //        else if (prop.PropertyType == typeof(DateTime))
        //        {
        //            field.Type = new string[]
        //            {
        //                "null",
        //                "string"
        //            };

        //            field.Default = null;

        //            continue;
        //        }
        //        else if (prop.PropertyType == typeof(DateTimeOffset))
        //        {
        //            field.Type = new string[]
        //            {
        //                "null",
        //                "string"
        //            };

        //            field.Default = null;

        //            continue;
        //        }
        //        else if (prop.PropertyType.IsEnum)
        //        {
        //            field.Type = new string[]
        //             {
        //                "null",
        //                "string"
        //             };

        //            field.Default = null;

        //            continue;
        //        }

        //        if (prop.PropertyType.IsArray)
        //        {
        //            string propTypelName = prop.PropertyType.Name.TrimEnd(new char[] { '[', ']' });

        //            if (prop.PropertyType.GetElementType().IsClass && propTypelName != "String")
        //            {
        //                //string propTypeFullName = prop.PropertyType.FullName.TrimEnd(new char[] { '[', ']' });
        //                //string propName = prop.PropertyType.Name.TrimEnd(new char[] { '[', ']' });

        //                var propNameSpace = prop.PropertyType.Namespace;
        //                var propFullName = prop.PropertyType.FullName;
        //                var propTypeName = propFullName.Replace(propNameSpace, String.Empty).TrimStart('.').TrimEnd(new char[] { '[', ']' });

        //                //var selected = dublicates.FirstOrDefault(x => x.Namespace + "." + x.Name == propTypeFullName);
        //                var selected = dublicates.FirstOrDefault(x => x.Name == propTypeName);

        //                if (selected == null)
        //                {
        //                    AvroSchema newAvro = new AvroSchema();
        //                    dynamic obj = new ExpandoObject();
        //                    obj.type = "array";
        //                    obj.items = newAvro;

        //                    field.Type = new dynamic[]
        //                    {
        //                     "null",
        //                     obj
        //                    };

        //                    field.Default = null;

        //                    dublicates.Add(newAvro);

        //                    newAvro.Type = "record";
        //                    newAvro.Name = prop.PropertyType.GetElementType().Name.TrimEnd(new char[] { '[', ']' });
        //                    newAvro.Namespace = prop.PropertyType.GetElementType().Namespace;

        //                    GetAvroPropertyCollectionUrban(prop.PropertyType.GetElementType(), newAvro, dublicates);
        //                }
        //                else
        //                {
        //                    dynamic objItems = new ExpandoObject();
        //                    dynamic obj = new ExpandoObject();
        //                    obj.type = "array";
        //                    obj.items = objItems;

        //                    objItems.name = propTypeName;
        //                    objItems.type = new string[]
        //                    {
        //                        "null",
        //                        propTypelName
        //                    };

        //                    IDictionary<string, object> dict = objItems;
        //                    dict["default"] = null;

        //                    field.Type = new dynamic[]
        //                    {
        //                     "null",
        //                      obj
        //                    };

        //                    field.Default = null;
        //                }

        //                continue;

        //            }
        //            else
        //            {
        //                dynamic expObj = new ExpandoObject();
        //                expObj.type = "array";

        //                if (propTypelName == "String")
        //                {
        //                    expObj.items = new string[]
        //                    {
        //                        "null",
        //                        "string"
        //                    };
        //                }
        //                else
        //                {
        //                    var tempField = SetAvroFieldForCollections(prop.PropertyType.GetElementType(), new AvroField());
        //                    expObj.items = tempField.Type;
        //                }

        //                field.Type = new dynamic[]
        //                {
        //                  "null",
        //                  expObj
        //                };

        //                field.Default = null;

        //                continue;
        //            }
        //        }
        //        if (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
        //        {
        //            if (prop.PropertyType.IsClass && prop.PropertyType.GetGenericArguments()[0].Name != "String")
        //            {
        //                //string propTypeFullName = prop.PropertyType.GetGenericArguments()[0].FullName.TrimEnd(new char[] { '[', ']' });

        //                var propNameSpace = prop.PropertyType.GetGenericArguments()[0].Namespace;
        //                var propFullName = prop.PropertyType.GetGenericArguments()[0].FullName;
        //                var propTypeName = propFullName.Replace(propNameSpace, String.Empty).TrimStart('.').TrimEnd(new char[] { '[', ']' });

        //                var selected = dublicates.FirstOrDefault(x => x.Name == propTypeName);

        //                if (selected == null)
        //                {
        //                    AvroSchema newAvro = new AvroSchema();
        //                    dynamic obj = new ExpandoObject();
        //                    obj.type = "array";
        //                    obj.items = newAvro;

        //                    field.Type = new dynamic[]
        //                    {
        //                      "null",
        //                      obj
        //                    };

        //                    field.Default = null;

        //                    dublicates.Add(newAvro);

        //                    newAvro.Type = "record";
        //                    newAvro.Name = prop.PropertyType.GenericTypeArguments[0].Name.TrimEnd(new char[] { '[', ']' });
        //                    newAvro.Namespace = prop.PropertyType.GetGenericArguments()[0].Namespace;

        //                    GetAvroPropertyCollectionUrban(prop.PropertyType.GetGenericArguments()[0], newAvro, dublicates);
        //                }
        //                else
        //                {
        //                    dynamic objItems = new ExpandoObject();
        //                    dynamic obj = new ExpandoObject();
        //                    obj.type = "array";
        //                    obj.items = objItems;
        //                    objItems.name = prop.Name;
        //                    objItems.type = new dynamic[]
        //                    {
        //                     "null",
        //                     propTypeName
        //                    };

        //                    IDictionary<string, object> dict = objItems;
        //                    dict["default"] = null;

        //                    field.Type = new dynamic[]
        //                    {
        //                     "null",
        //                      obj
        //                    };

        //                    field.Default = null;
        //                }

        //                continue;
        //            }
        //            else
        //            {
        //                var proprty = prop.PropertyType.GetGenericArguments()[0].Name.ToLower();

        //                dynamic obj = new ExpandoObject();
        //                obj.type = "array";
        //                obj.items = new string[]
        //                {
        //                    "null",
        //                    proprty
        //                };

        //                field.Type = new dynamic[]
        //                {
        //                     "null",
        //                      obj
        //                };

        //                field.Default = null;
        //            }

        //            continue;
        //        }
        //        else if (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Dictionary<,>)
        //             || prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(IDictionary<,>))
        //        {
        //            if (prop.PropertyType.GetGenericArguments()[0] == typeof(string))
        //            {
        //                if (!prop.PropertyType.GetGenericArguments()[1].IsClass || prop.PropertyType.GetGenericArguments()[1] == typeof(string))
        //                {
        //                    dynamic expObj = new ExpandoObject();
        //                    expObj.type = "map";
        //                    expObj.values = SetAvroFieldForCollections(prop.PropertyType.GetGenericArguments()[1], new AvroField());

        //                    field.Type = expObj;
        //                }
        //                else if (prop.PropertyType.GetGenericArguments()[1].IsClass && prop.PropertyType != typeof(string))
        //                {
        //                    var propNameSpace = prop.PropertyType.GetGenericArguments()[1].Namespace;
        //                    var propFullName = prop.PropertyType.GetGenericArguments()[1].FullName;
        //                    var propTypeName = propFullName.Replace(propNameSpace, String.Empty).TrimStart('.').TrimEnd(new char[] { '[', ']' });

        //                    var selected = dublicates.FirstOrDefault(x => x.Name == propTypeName);

        //                    //if (!dublicates.Select(x => x.Namespace).Contains(prop.PropertyType.FullName))
        //                    if (selected == null)
        //                    {
        //                        AvroSchema newAvro = new AvroSchema();
        //                        dynamic obj = new ExpandoObject();
        //                        obj.type = "map";
        //                        obj.values = newAvro;

        //                        field.Type = obj;

        //                        field.Default = null;

        //                        dublicates.Add(newAvro);
        //                        //Type tempType = prop.PropertyType;
        //                        GetAvroPropertyCollectionUrban(prop.PropertyType.GetGenericArguments()[1], newAvro, dublicates);
        //                        //GetPropertyCollection(tempType, dublicates, newAvro);
        //                    }
        //                    else
        //                    {
        //                        dynamic obj = new ExpandoObject();
        //                        obj.type = "record";
        //                        obj.values = new ExpandoObject();
        //                        obj.values.name = prop.Name;
        //                        obj.values.type = prop.PropertyType.GenericTypeArguments[1].Name;

        //                        dynamic expObj = new ExpandoObject();
        //                        expObj.type = "map";
        //                        expObj.values = obj;

        //                        field.Type = expObj;

        //                        //field.Default = null;
        //                    }
        //                }
        //            }

        //            continue;
        //        }
        //        else if (prop.PropertyType == typeof(Guid))
        //        {
        //            field.Type = new string[]
        //            {
        //                "null",
        //                "string"
        //            };

        //            field.Default = null;

        //            continue;
        //        }
        //        else if (prop.PropertyType == typeof(bool))
        //        {
        //            field.Type = "boolean";

        //            field.Default = false;

        //            continue;
        //        }
        //        else if (prop.PropertyType == typeof(double))
        //        {
        //            field.Type = "double";
        //            field.Default = 0.0;

        //            continue;
        //        }
        //        else if (prop.PropertyType == typeof(decimal))
        //        {
        //            field.Type = "double";
        //            field.Default = 0.0;

        //            continue;
        //        }
        //        else if (prop.PropertyType == typeof(float))
        //        {
        //            field.Type = "double";
        //            field.Default = 0.0;

        //            continue;
        //        }
        //        //Checking if is nullable
        //        else if (Nullable.GetUnderlyingType(prop.PropertyType) == typeof(int?)
        //              || Nullable.GetUnderlyingType(prop.PropertyType) == typeof(double?)
        //              || Nullable.GetUnderlyingType(prop.PropertyType) == typeof(decimal?)
        //              || Nullable.GetUnderlyingType(prop.PropertyType) == typeof(float?)
        //              || Nullable.GetUnderlyingType(prop.PropertyType) == typeof(long?)
        //              || Nullable.GetUnderlyingType(prop.PropertyType) == typeof(bool?)
        //              || Nullable.GetUnderlyingType(prop.PropertyType) == typeof(DateTimeOffset?)
        //              || Nullable.GetUnderlyingType(prop.PropertyType) == typeof(DateTime?)
        //              || Nullable.GetUnderlyingType(prop.PropertyType) == typeof(Guid?)
        //              || prop.PropertyType.IsGenericType &&
        //                    prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
        //        {
        //            Type u = Nullable.GetUnderlyingType(prop.PropertyType);
        //            if (u != null && u.IsEnum)
        //            {
        //                //TODO
        //                field.Type = new string[]
        //                {
        //                    "string",
        //                    "int"
        //                };

        //                field.Default = null;

        //                continue;
        //            }
        //            else if (prop.PropertyType == typeof(int?))
        //            {
        //                field.Type = new string[]
        //                {
        //                    "null",
        //                    "int"
        //                };

        //                field.Default = null;

        //                continue;
        //            }
        //            else if (prop.PropertyType == typeof(long?))
        //            {
        //                field.Type = new string[]
        //                {
        //                    "null",
        //                    "long"
        //                };

        //                field.Default = null;

        //                continue;
        //            }
        //            else if (prop.PropertyType == typeof(DateTime?))
        //            {
        //                field.Type = new string[]
        //                {
        //                    "null",
        //                    "string"
        //                };

        //                field.Default = null;

        //                continue;
        //            }
        //            else if (prop.PropertyType == typeof(DateTimeOffset?))
        //            {
        //                field.Type = new string[]
        //                {
        //                    "null",
        //                    "string"
        //                };

        //                field.Default = null;

        //                continue;
        //            }
        //            else if (prop.PropertyType == typeof(Guid?))
        //            {
        //                field.Type = new string[]
        //                {
        //                    "null",
        //                    "string"
        //                };

        //                field.Default = null;

        //                continue;
        //            }
        //            else if (prop.PropertyType == typeof(bool?))
        //            {
        //                field.Type = "boolean";

        //                field.Default = false;

        //                continue;
        //            }
        //            else if (prop.PropertyType == typeof(double?))
        //            {
        //                field.Type = new string[]
        //                {
        //                "null",
        //                "double"
        //                };

        //                field.Default = null;

        //                continue;
        //            }
        //            else if (prop.PropertyType == typeof(decimal?))
        //            {
        //                field.Type = new string[]
        //                {
        //                "null",
        //                "double"
        //                };

        //                field.Default = null;

        //                continue;
        //            }
        //            else if (prop.PropertyType == typeof(float?))
        //            {
        //                field.Type = new string[]
        //                {
        //                "null",
        //                "double"
        //                };

        //                field.Default = null;

        //                continue;
        //            }
        //            else if (prop.PropertyType.IsClass && prop.PropertyType != typeof(string))
        //            {
        //                string propTypeFullName = prop.PropertyType.FullName;
        //                string propTypeFullName21 = prop.PropertyType.Namespace;

        //                var selected = dublicates.FirstOrDefault(x => x.Namespace + "." + x.Name == propTypeFullName);

        //                if (selected == null)
        //                {
        //                    AvroSchema newAvro = new AvroSchema();

        //                    field.Type = new dynamic[]
        //                    {
        //                     "null",
        //                     newAvro,
        //                    };

        //                    dublicates.Add(newAvro);

        //                    newAvro.Type = "record";
        //                    newAvro.Name = prop.PropertyType.Name.TrimEnd(new char[] { '[', ']' });
        //                    newAvro.Namespace = prop.PropertyType.Namespace;

        //                    GetAvroPropertyCollectionUrban(prop.PropertyType, newAvro, dublicates);
        //                }
        //                else
        //                {
        //                    field.Type = new dynamic[]
        //                   {
        //                    "null",
        //                    $"{prop.PropertyType.Namespace}.{prop.PropertyType.Name}"
        //                   };

        //                    field.Default = null;
        //                    continue;
        //                }
        //            }
        //        }
        //        else if (prop.PropertyType.IsClass && prop.PropertyType != typeof(string))
        //        {
        //            string propTypeFullName = prop.PropertyType.FullName;

        //            var selected = dublicates.FirstOrDefault(x => x.Namespace + "." + x.Name == propTypeFullName);

        //            if (selected == null)
        //            {
        //                AvroSchema newAvro = new AvroSchema();

        //                field.Type = new dynamic[]
        //                {
        //                 "null",
        //                 newAvro,
        //                };

        //                dublicates.Add(newAvro);

        //                newAvro.Type = "record";
        //                newAvro.Name = prop.PropertyType.Name.TrimEnd(new char[] { '[', ']' });
        //                newAvro.Namespace = prop.PropertyType.Namespace;

        //                GetAvroPropertyCollectionUrban(prop.PropertyType, newAvro, dublicates);
        //            }
        //            else
        //            {
        //                field.Type = new dynamic[]
        //               {
        //                "null",
        //                $"{prop.PropertyType.Namespace}.{prop.PropertyType.Name}"
        //               };

        //                field.Default = null;
        //                continue;
        //            }
        //        }

        //    }
        //    return schema;
        //}

        //public AvroField SetAvroFieldForCollections(Type prop, dynamic field)
        //{
        //    if (prop == typeof(int))
        //    {
        //        field.Type = "int";
        //        field.Default = 0;
        //    }
        //    if (prop == typeof(long))
        //    {
        //        field.Type = "long";
        //        field.Default = 0;
        //    }
        //    else if (prop == typeof(string))
        //    {
        //        field.Type = new string[]
        //        {
        //          "null",
        //          "string"
        //        };

        //        field.Default = null;
        //    }
        //    else if (prop == typeof(DateTime))
        //    {
        //        field.Type = new string[]
        //        {
        //          "null",
        //          "string"
        //        };

        //        field.Default = null;
        //    }
        //    else if (prop == typeof(DateTimeOffset))
        //    {
        //        field.Type = new string[]
        //        {
        //           "null",
        //           "string"
        //        };

        //        field.Default = null;
        //    }
        //    else if (prop.IsEnum)
        //    {
        //        field.Type = new string[]
        //         {
        //           "null",
        //           "string"
        //         };

        //        field.Default = null;
        //    }
        //    else if (prop == typeof(Guid))
        //    {
        //        field.Type = new string[]
        //        {
        //          "null",
        //          "string"
        //        };

        //        field.Default = null;
        //    }
        //    else if (prop == typeof(bool))
        //    {
        //        field.Type = "boolean";

        //        field.Default = false;
        //    }
        //    else if (prop == typeof(double))
        //    {
        //        field.Type = "double";
        //        field.Default = 0.0;
        //    }
        //    else if (prop == typeof(decimal))
        //    {
        //        field.Type = "double";
        //        field.Default = 0.0;
        //    }
        //    else if (prop == typeof(float))
        //    {
        //        field.Type = "double";
        //        field.Default = 0.0;
        //    }
        //    //Checking if is nullable
        //    else if (Nullable.GetUnderlyingType(prop) == typeof(int?)
        //          || Nullable.GetUnderlyingType(prop) == typeof(double?)
        //          || Nullable.GetUnderlyingType(prop) == typeof(decimal?)
        //          || Nullable.GetUnderlyingType(prop) == typeof(float?)
        //          || Nullable.GetUnderlyingType(prop) == typeof(long?)
        //          || Nullable.GetUnderlyingType(prop) == typeof(bool?)
        //          || Nullable.GetUnderlyingType(prop) == typeof(DateTimeOffset?)
        //          || Nullable.GetUnderlyingType(prop) == typeof(DateTime?)
        //          || Nullable.GetUnderlyingType(prop) == typeof(Guid?)
        //          || prop.IsGenericType &&
        //                prop.GetGenericTypeDefinition() == typeof(Nullable<>))
        //    {
        //        Type u = Nullable.GetUnderlyingType(prop);
        //        if (u != null && u.IsEnum)
        //        {
        //            //TODO
        //            field.Type = new string[]
        //            {
        //              "string",
        //              "int"
        //            };

        //            field.Default = null;

        //        }
        //        else if (prop == typeof(int?))
        //        {
        //            field.Type = new string[]
        //            {
        //              "null",
        //              "int"
        //            };

        //            field.Default = null;
        //        }
        //        else if (prop == typeof(long?))
        //        {
        //            field.Type = new string[]
        //            {
        //              "null",
        //              "long"
        //            };

        //            field.Default = null;
        //        }
        //        else if (prop == typeof(DateTime?))
        //        {
        //            field.Type = new string[]
        //            {
        //              "null",
        //              "string"
        //            };

        //            field.Default = null;
        //        }
        //        else if (prop == typeof(DateTimeOffset?))
        //        {
        //            field.Type = new string[]
        //            {
        //              "null",
        //              "string"
        //            };

        //            field.Default = null;
        //        }
        //        else if (prop == typeof(Guid?))
        //        {
        //            field.Type = new string[]
        //            {
        //              "null",
        //              "string"
        //            };

        //            field.Default = null;
        //        }
        //        else if (prop == typeof(bool?))
        //        {
        //            field.Type = "boolean";

        //            field.Default = false;
        //        }
        //        else if (prop == typeof(double?))
        //        {
        //            field.Type = new string[]
        //            {
        //              "null",
        //              "double"
        //            };

        //            field.Default = null;
        //        }
        //        else if (prop == typeof(decimal?))
        //        {
        //            field.Type = new string[]
        //            {
        //              "null",
        //              "double"
        //            };

        //            field.Default = null;
        //        }
        //        else if (prop == typeof(float?))
        //        {
        //            field.Type = new string[]
        //            {
        //              "null",
        //              "double"
        //            };

        //            field.Default = null;
        //        }

        //    }

        //    return field;
        //}


        //private ClassLayout GetClassPropertyCollection(PropertyInfo[] properties, ClassLayout classLayout, List<ClassLayout> classDublicates, ClassGenerateSettings settings = null)
        //{
        //    var dictionary = new Dictionary<string, dynamic>();
        //    classLayout.Properties = dictionary;
        //    classLayout.ClassCollection = new List<ClassLayout>();

        //    foreach (var prop in properties)
        //    {
        //        if (prop.PropertyType == typeof(int))
        //        {
        //            dictionary[prop.Name] = "System.Int32";
        //            continue;
        //        }
        //        if (prop.PropertyType == typeof(long))
        //        {
        //            dictionary[prop.Name] = "System.Int64";
        //            continue;
        //        }
        //        else if (prop.PropertyType == typeof(string))
        //        {
        //            dictionary[prop.Name] = "System.String";
        //            continue;
        //        }
        //        else if (prop.PropertyType == typeof(DateTime))
        //        {
        //            dictionary[prop.Name] = "System.String";
        //            continue;
        //        }
        //        else if (prop.PropertyType == typeof(DateTimeOffset))
        //        {
        //            dictionary[prop.Name] = "System.String";
        //            continue;
        //        }
        //        else if (prop.PropertyType.IsEnum)
        //        {
        //            if (settings != null)
        //            {
        //                if (settings.EnumConvert == Enums.EnumConvert.Int)
        //                {
        //                    dictionary[prop.Name] = "System.Int32";
        //                    continue;
        //                }
        //                else
        //                {
        //                    dictionary[prop.Name] = "System.String";
        //                    continue;
        //                }
        //            }
        //            else
        //            {
        //                dictionary[prop.Name] = "System.String";
        //                continue;
        //            }
        //        }
        //        if (prop.PropertyType.IsArray)
        //        {
        //            string propTypelName = prop.PropertyType.Name.TrimEnd(new char[] { '[', ']' });
        //            dictionary[prop.Name] = $"{prop.PropertyType}";

        //            if (prop.PropertyType.GetElementType().IsClass && propTypelName != "String")
        //            {
        //                dictionary[prop.Name] = $"{prop.PropertyType.Name}";

        //                DeclareAsClass(prop.PropertyType.GetElementType().Name,
        //                                prop.PropertyType.GetElementType().FullName,
        //                                prop.PropertyType.GetElementType().Name,
        //                                prop.PropertyType.GetElementType(),
        //                                classLayout,
        //                                dictionary,
        //                                classDublicates,
        //                                settings,
        //                                false);
        //            }
        //            continue;

        //            //else
        //            //{
        //            //    string propName = prop.PropertyType.GetElementType().Name.TrimEnd(new char[] { '[', ']' });
        //            //    string propFullName = prop.PropertyType.GetElementType().FullName.TrimEnd(new char[] { '[', ']' });
        //            //
        //            //    DeclareAsClass(propName, propFullName, propName, prop.PropertyType.GetElementType(), classLayout, dictionary, classDublicates, false);
        //            //
        //            //    continue;
        //            //}
        //        }
        //        else if (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
        //        {
        //            if (prop.PropertyType.GetGenericArguments()[0] == typeof(Nullable<>))
        //            {

        //            }

        //            dictionary[prop.Name] = $"List<{prop.PropertyType.GenericTypeArguments[0].Name}>";

        //            if (prop.PropertyType.IsClass && prop.PropertyType.GetGenericArguments()[0].Name != "String")
        //            {
        //                DeclareAsClass(prop.PropertyType.GenericTypeArguments[0].Name,
        //                            prop.PropertyType.GenericTypeArguments[0].FullName,
        //                            prop.PropertyType.GenericTypeArguments[0].Name,
        //                            prop.PropertyType.GenericTypeArguments[0],
        //                            classLayout,
        //                            dictionary,
        //                            classDublicates,
        //                            settings,
        //                            false);
        //            }

        //            continue;
        //        }

        //        else if (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Dictionary<,>)
        //            || prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(IDictionary<,>))
        //        {
        //            if (prop.PropertyType.GetGenericArguments()[0].IsClass && prop.PropertyType.GetGenericArguments()[0].Name != "String")
        //            {
        //                ClassLayout dicClass = new ClassLayout
        //                {
        //                    Name = prop.Name,
        //                    FullName = prop.PropertyType.FullName,
        //                    Type = prop.Name,
        //                    CreateAsProperty = true,
        //                    Properties = new Dictionary<string, dynamic>(),
        //                    ClassCollection = new List<ClassLayout>()
        //                };

        //                classLayout.ClassCollection.Add(dicClass);

        //                dictionary[prop.Name] = dicClass;

        //                DeclareAsClass(prop.PropertyType.GetGenericArguments()[0].Name,
        //                                   prop.PropertyType.GetGenericArguments()[0].FullName,
        //                                   prop.PropertyType.GetGenericArguments()[0].Name,
        //                                   prop.PropertyType.GetGenericArguments()[0],
        //                                   classLayout,
        //                                   dictionary,
        //                                   classDublicates,
        //                                   settings,
        //                                   false);

        //                dicClass.Properties.Add("Key", prop.PropertyType.GetGenericArguments()[0].Name);


        //                if (prop.PropertyType.GetGenericArguments()[1].IsClass && prop.PropertyType.GetGenericArguments()[1] != typeof(string))
        //                {
        //                    DeclareAsClass(prop.PropertyType.GetGenericArguments()[1].Name,
        //                                   prop.PropertyType.GetGenericArguments()[1].FullName,
        //                                   prop.PropertyType.GetGenericArguments()[1].Name,
        //                                   prop.PropertyType.GetGenericArguments()[1],
        //                                   classLayout,
        //                                   dictionary,
        //                                   classDublicates,
        //                                   settings,
        //                                   false);

        //                    dicClass.Properties.Add("Value", prop.PropertyType.GetGenericArguments()[1].Name);
        //                }
        //                else
        //                {
        //                    dicClass.Properties.Add("Value", GetClassPropertyType(prop.PropertyType.GetGenericArguments()[1], settings));
        //                }

        //                continue;
        //            }
        //            else
        //            {
        //                dictionary[prop.Name] = $"Dictionary<string, {GetClassPropertyType(prop.PropertyType.GetGenericArguments()[1], settings)}>";

        //                if (prop.PropertyType.GetGenericArguments()[1].IsClass && prop.PropertyType.GetGenericArguments()[1] != typeof(string))
        //                {
        //                    DeclareAsClass(prop.PropertyType.GetGenericArguments()[1].Name,
        //                                   prop.PropertyType.GetGenericArguments()[1].FullName,
        //                                   prop.PropertyType.GetGenericArguments()[1].Name,
        //                                   prop.PropertyType.GetGenericArguments()[1],
        //                                   classLayout,
        //                                   dictionary,
        //                                   classDublicates,
        //                                   settings,
        //                                   false);
        //                }
        //            }

        //            continue;
        //        }
        //        else if (prop.PropertyType == typeof(Guid))
        //        {
        //            dictionary[prop.Name] = "System.String";
        //            continue;
        //        }
        //        else if (prop.PropertyType == typeof(bool))
        //        {
        //            dictionary[prop.Name] = "System.Boolean";
        //            continue;
        //        }
        //        else if (prop.PropertyType == typeof(double))
        //        {
        //            dictionary[prop.Name] = "System.Double";
        //            continue;
        //        }
        //        else if (prop.PropertyType == typeof(decimal))
        //        {
        //            dictionary[prop.Name] = "System.Double";
        //            continue;
        //        }
        //        else if (prop.PropertyType == typeof(float))
        //        {
        //            dictionary[prop.Name] = "System.Double";

        //            continue;
        //        }
        //        //Checking if is nullable
        //        else if (Nullable.GetUnderlyingType(prop.PropertyType) == typeof(int?)
        //              || Nullable.GetUnderlyingType(prop.PropertyType) == typeof(double?)
        //              || Nullable.GetUnderlyingType(prop.PropertyType) == typeof(decimal?)
        //              || Nullable.GetUnderlyingType(prop.PropertyType) == typeof(float?)
        //              || Nullable.GetUnderlyingType(prop.PropertyType) == typeof(long?)
        //              || Nullable.GetUnderlyingType(prop.PropertyType) == typeof(bool?)
        //              || Nullable.GetUnderlyingType(prop.PropertyType) == typeof(DateTimeOffset?)
        //              || Nullable.GetUnderlyingType(prop.PropertyType) == typeof(DateTime?)
        //              || Nullable.GetUnderlyingType(prop.PropertyType) == typeof(Guid?)
        //              || prop.PropertyType.IsGenericType &&
        //                    prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
        //        {
        //            Type u = Nullable.GetUnderlyingType(prop.PropertyType);
        //            if (u != null && u.IsEnum)
        //            {
        //                if (settings != null)
        //                {
        //                    if (settings.EnumConvert == Enums.EnumConvert.String)
        //                    {
        //                        dictionary[prop.Name] = "int?";
        //                        continue;
        //                    }
        //                    else
        //                    {
        //                        dictionary[prop.Name] = "System.String";
        //                        continue;
        //                    }
        //                }
        //                else
        //                {
        //                    dictionary[prop.Name] = "System.String";
        //                    continue;
        //                }
        //            }
        //            else if (prop.PropertyType == typeof(int?))
        //            {
        //                dictionary[prop.Name] = "int?";
        //                continue;
        //            }
        //            else if (prop.PropertyType == typeof(long?))
        //            {
        //                dictionary[prop.Name] = "long?";
        //                continue;
        //            }
        //            else if (prop.PropertyType == typeof(DateTime?))
        //            {
        //                dictionary[prop.Name] = "System.String";
        //                continue;
        //            }
        //            else if (prop.PropertyType == typeof(DateTimeOffset?))
        //            {
        //                dictionary[prop.Name] = "System.String";
        //                continue;
        //            }
        //            else if (prop.PropertyType == typeof(Guid?))
        //            {
        //                dictionary[prop.Name] = "System.String";
        //                continue;
        //            }
        //            else if (prop.PropertyType == typeof(bool?))
        //            {
        //                dictionary[prop.Name] = "System.Boolean?";
        //                continue;
        //            }
        //            else if (prop.PropertyType == typeof(double?))
        //            {
        //                dictionary[prop.Name] = "double?";
        //                continue;
        //            }
        //            else if (prop.PropertyType == typeof(decimal?))
        //            {
        //                dictionary[prop.Name] = "double?";
        //                continue;
        //            }
        //            else if (prop.PropertyType == typeof(float?))
        //            {
        //                dictionary[prop.Name] = "double?";
        //                continue;
        //            }
        //            else if (prop.PropertyType.IsClass && prop.PropertyType != typeof(string))
        //            {
        //                dictionary[prop.Name] = prop.PropertyType.Name;
        //                DeclareAsClass(prop.Name, prop.PropertyType.FullName, prop.PropertyType.Name, prop.PropertyType, classLayout, dictionary, classDublicates, settings);

        //                continue;
        //            }
        //        }
        //        else if (prop.PropertyType.IsClass && prop.PropertyType != typeof(string))
        //        {
        //            dictionary[prop.Name] = prop.PropertyType.Name;
        //            //dictionary[prop.Name] = prop.Name;
        //            DeclareAsClass(prop.Name, prop.PropertyType.FullName, prop.PropertyType.Name, prop.PropertyType, classLayout, dictionary, classDublicates, settings);

        //            continue;
        //        }
        //    }

        //    return classLayout;
        //}

        //private ClassLayout DeclareAsClass(string className, string classFullName, string classTypeName, Type propType, ClassLayout parentClass, Dictionary<string, dynamic> dictionary, List<ClassLayout> classDublicates, ClassGenerateSettings settings = null, bool createAsProperty = true)
        //{
        //    ClassLayout classLayout = new ClassLayout();
        //    classLayout.Name = className;
        //    classLayout.FullName = classFullName;
        //    classLayout.Type = classTypeName;
        //    classLayout.CreateAsProperty = createAsProperty;
        //    classLayout.Properties = new Dictionary<string, dynamic>();

        //    //dictionary[className] = classLayout;

        //    parentClass.ClassCollection.Add(classLayout);

        //    var getDublicateClass = classDublicates.FirstOrDefault(x => x.FullName == classLayout.FullName);
        //    if (getDublicateClass == null)
        //    {
        //        if (classDublicates.Select(x => x.Type).Contains(classLayout.Type))
        //        {
        //            classLayout.Type = className;
        //            dictionary[className] = classLayout.Type;
        //        }

        //        classDublicates.Add(classLayout);
        //        Type tempType = propType;
        //        GetClassPropertyCollectionUrban(tempType, classLayout, classDublicates, settings);
        //    }
        //    else
        //    {
        //        classLayout.Properties = getDublicateClass.Properties;
        //        classLayout.ClassCollection = getDublicateClass.ClassCollection;
        //    }

        //    return classLayout;
        //}

        //private string GetClassPropertyType(Type type, ClassGenerateSettings settings)
        //{
        //    if (type == typeof(int))
        //    {
        //        return "int";
        //    }
        //    if (type == typeof(long))
        //    {
        //        return "long";
        //    }
        //    else if (type == typeof(string))
        //    {
        //        return "string";
        //    }
        //    else if (type == typeof(DateTime))
        //    {
        //        return "string";
        //    }
        //    else if (type == typeof(DateTimeOffset))
        //    {
        //        return "string";
        //    }
        //    else if (type.IsEnum)
        //    {
        //        if (settings != null)
        //        {
        //            if (settings.EnumConvert == Enums.EnumConvert.Int)
        //            {
        //                return "int";
        //            }
        //            else
        //            {
        //                return "string";
        //            }
        //        }
        //        else
        //        {
        //            return "string";
        //        }
        //    }
        //    else if (type == typeof(Guid))
        //    {
        //        return "string";
        //    }
        //    else if (type == typeof(bool))
        //    {
        //        return "bool";
        //    }
        //    else if (type == typeof(double))
        //    {
        //        return "double";
        //    }
        //    else if (type == typeof(decimal))
        //    {
        //        return "double";
        //    }
        //    else if (type == typeof(float))
        //    {
        //        return "double";
        //    }
        //    else if (type.IsClass)
        //    {
        //        return type.Name;
        //    }
        //    //Checking if is nullable
        //    else if (Nullable.GetUnderlyingType(type) == typeof(int?)
        //          || Nullable.GetUnderlyingType(type) == typeof(double?)
        //          || Nullable.GetUnderlyingType(type) == typeof(decimal?)
        //          || Nullable.GetUnderlyingType(type) == typeof(float?)
        //          || Nullable.GetUnderlyingType(type) == typeof(long?)
        //          || Nullable.GetUnderlyingType(type) == typeof(bool?)
        //          || Nullable.GetUnderlyingType(type) == typeof(DateTimeOffset?)
        //          || Nullable.GetUnderlyingType(type) == typeof(DateTime?)
        //          || Nullable.GetUnderlyingType(type) == typeof(Guid?)
        //          || type.IsGenericType &&
        //               type.GetGenericTypeDefinition() == typeof(Nullable<>))
        //    {
        //        Type u = Nullable.GetUnderlyingType(type);

        //        if (u != null && u.IsEnum)
        //        {
        //            if (settings != null)
        //            {
        //                if (settings.EnumConvert == Enums.EnumConvert.Int)
        //                {
        //                    return "int?";
        //                }
        //                else
        //                {
        //                    return "string";
        //                }
        //            }
        //            else
        //            {
        //                return "string";
        //            }
        //        }
        //        else if (type == typeof(int?))
        //        {
        //            return "int?";
        //        }
        //        else if (type == typeof(long?))
        //        {
        //            return "long?";
        //        }
        //        else if (type == typeof(DateTime?))
        //        {
        //            return "string";
        //        }
        //        else if (type == typeof(DateTimeOffset?))
        //        {
        //            return "string";
        //        }
        //        else if (type == typeof(Guid?))
        //        {
        //            return "string";
        //        }
        //        else if (type == typeof(bool?))
        //        {
        //            return "bool?";
        //        }
        //        else if (type == typeof(double?))
        //        {
        //            return "double?";
        //        }
        //        else if (type == typeof(decimal?))
        //        {
        //            return "double?";
        //        }
        //        else if (type == typeof(float?))
        //        {
        //            return "double?";
        //        }
        //        else if (type.IsClass)
        //        {
        //            return type.Name + "?";
        //        }
        //        else
        //        {
        //            return "undifinied";
        //        }
        //    }
        //    else
        //    {
        //        return "undifinied";
        //    }
        //}

    }
}
