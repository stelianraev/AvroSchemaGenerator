﻿namespace AutomatizationVersionUpdate.Services
{
    using AutomatizationVersionUpdate.AvroService.Models;
    using System.Dynamic;
    using System.Reflection;

    public abstract class Services
    {
        /// <summary>
        /// Save File in working directory
        /// </summary>
        /// <param name="file">given file as string input</param>
        /// <param name="fileName">name of file</param>
        /// <param name="fileExtension">file type as .cs, .json, .avsc and etc. there is always . so input must be just name of extension without . </param>
        /// <param name="directories">directory separated with . (example: MytestFolder\MyNestedTestFolder\WorkingFolder) this will return nested folders and inside the class</param>
        public void WriteFile(string file, string fileName, string fileExtension, string? directories, string? exactPath = null)
        {
            // Create a new CodeNamespace and add it to the CodeCompileUnit
            //var myType = typeof(GenerateClassDynamically<T>);

            var directory = directories.Split("\\", StringSplitOptions.RemoveEmptyEntries).Aggregate((x, y) => Path.Combine(x, y));

            // Write the code to a file
            var path = FilePathFromNameSpace(fileName + "." + fileExtension, directory, exactPath);
            File.WriteAllText(path, file);
        }

        /// <summary>
        /// FilePath in Working directory with exact path
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="directory"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        private string FilePathFromNameSpace(string fileName, string directory, string? path)
        {
            // This will get the current WORKING directory (i.e. \bin\Debug)
            string workingDirectory = Environment.CurrentDirectory;

            // This will get the current PROJECT bin directory (ie ../bin/)
            //string projectDirectory = Directory.GetParent(workingDirectory).Parent.FullName;

            // This will get the current PROJECT directory
            string projectDirectoryy;
            if (String.IsNullOrEmpty(path) || String.IsNullOrWhiteSpace(path))
            {
                projectDirectoryy = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
            }
            else
            {
                projectDirectoryy = path;
            }

            //var folders = nameSpace.Split(".", StringSplitOptions.RemoveEmptyEntries)
            //    .Aggregate((x, y) => System.IO.Path.Combine(x, y));

            var pathCombine = Path.Combine(projectDirectoryy, directory);

            //var path = Path.Combine(projectDirectoryy, folderName, NestedFolderName);

            if (!Directory.Exists(pathCombine))
            {
                Directory.CreateDirectory(pathCombine);
            }

            return Path.Combine(pathCombine, fileName);
        }

        /// <summary>
        /// Set AvroSchema class and set schema fields
        /// </summary>
        /// <param name="type"></param>
        /// <param name="dublicates"></param>
        /// <param name="schema"></param>
        /// <param name="ignoreSchemaDefault"></param>
        /// <returns></returns>
        public AvroSchema GetAvroPropertyCollection(Type type, List<AvroSchema> dublicates, AvroSchema schema, bool ignoreSchemaDefault = false)
        {
            string typeName = type.Name.TrimEnd(new char[] { '[', ']' });         

            schema.Type = "record";
            schema.Name = typeName;
            schema.Namespace = type.Namespace;

            if (ignoreSchemaDefault)
            {
                schema.Default = "ignore";
            }

            schema.Fields = new List<AvroField>();

            PropertyInfo[] properties = type.GetProperties();

            foreach (var prop in properties)
            {
                var field = new AvroField();
                field.Name = prop.Name;

                schema.Fields.Add(field);

                if (prop.PropertyType == typeof(int))
                {
                    field.Type = "int";
                    field.Default = 0;

                    continue;
                }
                if (prop.PropertyType == typeof(long))
                {
                    field.Type = "long";
                    field.Default = 0;
                }
                else if (prop.PropertyType == typeof(string))
                {
                    field.Type = new string[]
                    {
                        "null",
                        "string"
                    };

                    field.Default = null;

                    continue;
                }
                else if (prop.PropertyType == typeof(DateTime))
                {
                    field.Type = new string[]
                    {
                        "null",
                        "string"
                    };

                    field.Default = null;

                    continue;
                }
                else if (prop.PropertyType == typeof(DateTimeOffset))
                {
                    field.Type = new string[]
                    {
                        "null",
                        "string"
                    };

                    field.Default = null;

                    continue;
                }
                else if (prop.PropertyType.IsEnum)
                {
                    field.Type = new string[]
                     {
                        "null",
                        "string"
                     };

                    field.Default = null;

                    continue;
                }

                if (prop.PropertyType.IsArray)
                {
                    string propTypelName = prop.PropertyType.Name.TrimEnd(new char[] { '[', ']' });

                    if (prop.PropertyType.GetElementType().IsClass && propTypelName != "String")
                    {
                        //string propTypeFullName = prop.PropertyType.FullName.TrimEnd(new char[] { '[', ']' });
                        //string propName = prop.PropertyType.Name.TrimEnd(new char[] { '[', ']' });

                        var propNameSpace = prop.PropertyType.Namespace;
                        var propFullName = prop.PropertyType.FullName;
                        var propTypeName = propFullName.Replace(propNameSpace, String.Empty).TrimStart('.').TrimEnd(new char[] { '[', ']' });

                        //var selected = dublicates.FirstOrDefault(x => x.Namespace + "." + x.Name == propTypeFullName);
                        var selected = dublicates.FirstOrDefault(x => x.Name == propTypeName);

                        if (selected == null)
                        {
                            AvroSchema newAvro = new AvroSchema();

                            dynamic obj = new ExpandoObject();
                            obj.type = "array";
                            obj.items = newAvro;

                            field.Type = new dynamic[]
                            {
                             "null",
                              obj
                            };

                            field.Default = null;

                            dublicates.Add(newAvro);

                            newAvro.Type = "record";
                            newAvro.Name = prop.PropertyType.GetElementType().Name.TrimEnd(new char[] { '[', ']' });
                            newAvro.Namespace = prop.PropertyType.GetElementType().Namespace;

                            GetAvroPropertyCollection(prop.PropertyType.GetElementType(), dublicates, newAvro, ignoreSchemaDefault);
                        }
                        else
                        {
                            dynamic objItems = new ExpandoObject();
                            dynamic obj = new ExpandoObject();
                            obj.type = "array";
                            obj.items = objItems;

                            objItems.name = propTypeName;
                            objItems.type = new string[]
                            {
                                "null",
                                propTypelName
                            };

                            IDictionary<string, object> dict = objItems;
                            dict["default"] = null;

                            field.Type = new dynamic[]
                            {
                             "null",
                              obj
                            };

                            field.Default = null;
                        }

                        continue;

                    }
                    else
                    {
                        dynamic expObj = new ExpandoObject();
                        expObj.type = "array";

                        if (propTypelName == "String")
                        {
                            expObj.items = new string[]
                            {
                                "null",
                                "string"
                            };
                        }
                        else
                        {
                            var tempField = SetAvroFieldForCollections(prop.PropertyType.GetElementType(), new AvroField());
                            expObj.items = tempField.Type;
                        }

                        field.Type = new dynamic[]
                        {
                          "null",
                          expObj
                        };

                        field.Default = null;

                        continue;
                    }
                }
                if (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    if (prop.PropertyType.IsClass && prop.PropertyType.GetGenericArguments()[0].Name != "String")
                    {
                        //string propTypeFullName = prop.PropertyType.GetGenericArguments()[0].FullName.TrimEnd(new char[] { '[', ']' });
                        var propNameSpace = prop.PropertyType.GetGenericArguments()[0].Namespace;
                        var propFullName = prop.PropertyType.GetGenericArguments()[0].FullName;

                        var propTypeName = propFullName.Replace(propNameSpace, String.Empty).TrimStart('.').TrimEnd(new char[] { '[', ']' });

                        var selected = dublicates.FirstOrDefault(x => x.Name == propTypeName);

                        if (selected == null)
                        {
                            AvroSchema newAvro = new AvroSchema();
                            dynamic obj = new ExpandoObject();
                            obj.type = "array";
                            obj.items = newAvro;

                            field.Type = new dynamic[]
                            {
                              "null",
                              obj
                            };

                            field.Default = null;

                            dublicates.Add(newAvro);

                            newAvro.Type = "record";
                            newAvro.Name = prop.PropertyType.GenericTypeArguments[0].Name.TrimEnd(new char[] { '[', ']' });
                            newAvro.Namespace = prop.PropertyType.GetGenericArguments()[0].Namespace;

                            GetAvroPropertyCollection(prop.PropertyType.GetGenericArguments()[0], dublicates, newAvro, ignoreSchemaDefault);
                        }
                        else
                        {
                            dynamic objItems = new ExpandoObject();
                            dynamic obj = new ExpandoObject();
                            obj.type = "array";
                            obj.items = objItems;
                            objItems.name = prop.Name;
                            objItems.type = new dynamic[]
                            {
                             "null",
                             propTypeName
                            };

                            IDictionary<string, object> dict = objItems;
                            dict["default"] = null;

                            field.Type = new dynamic[]
                            {
                             "null",
                              obj
                            };

                            field.Default = null;
                        }

                        continue;
                    }
                    else
                    {
                        var proprty = prop.PropertyType.GetGenericArguments()[0].Name.ToLower();

                        dynamic obj = new ExpandoObject();
                        obj.type = "array";
                        obj.items = new string[]
                        {
                            "null",
                            proprty
                        };

                        field.Type = new dynamic[]
                        {
                             "null",
                              obj
                        };

                        field.Default = null;
                    }

                    continue;
                }
                else if (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Dictionary<,>)
                     || prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(IDictionary<,>))
                {
                    if (prop.PropertyType.GetGenericArguments()[0] == typeof(string))
                    {
                        if (!prop.PropertyType.GetGenericArguments()[1].IsClass || prop.PropertyType.GetGenericArguments()[1] == typeof(string))
                        {
                            dynamic expObj = new ExpandoObject();
                            expObj.type = "map";
                            expObj.values = SetAvroFieldForCollections(prop.PropertyType.GetGenericArguments()[1], new AvroField());

                            field.Type = expObj;
                        }
                        else if (prop.PropertyType.GetGenericArguments()[1].IsClass && prop.PropertyType != typeof(string))
                        {
                            var propNameSpace = prop.PropertyType.GetGenericArguments()[1].Namespace;
                            var propFullName = prop.PropertyType.GetGenericArguments()[1].FullName;
                            var propTypeName = propFullName.Replace(propNameSpace, String.Empty).TrimStart('.').TrimEnd(new char[] { '[', ']' });

                            var selected = dublicates.FirstOrDefault(x => x.Name == propTypeName);

                            //if (!dublicates.Select(x => x.Namespace).Contains(prop.PropertyType.FullName))
                            if (selected == null)
                            {
                                AvroSchema newAvro = new AvroSchema();
                                dynamic obj = new ExpandoObject();
                                obj.type = "map";
                                obj.values = newAvro;

                                field.Type = obj;

                                field.Default = null;

                                dublicates.Add(newAvro);
                                //Type tempType = prop.PropertyType;
                                GetAvroPropertyCollection(prop.PropertyType.GetGenericArguments()[1], dublicates, newAvro, ignoreSchemaDefault);
                                //GetPropertyCollection(tempType, dublicates, newAvro);
                            }
                            else
                            {
                                dynamic obj = new ExpandoObject();
                                obj.type = "record";
                                obj.values = new ExpandoObject();
                                obj.values.name = prop.Name;
                                obj.values.type = prop.PropertyType.GenericTypeArguments[1].Name;

                                dynamic expObj = new ExpandoObject();
                                expObj.type = "map";
                                expObj.values = obj;

                                field.Type = expObj;

                                //field.Default = null;
                            }
                        }
                    }

                    continue;
                }
                else if (prop.PropertyType == typeof(Guid))
                {
                    field.Type = new string[]
                    {
                        "null",
                        "string"
                    };

                    field.Default = null;

                    continue;
                }
                else if (prop.PropertyType == typeof(bool))
                {
                    field.Type = "boolean";

                    field.Default = false;

                    continue;
                }
                else if (prop.PropertyType == typeof(double))
                {
                    field.Type = "double";
                    field.Default = 0.0;

                    continue;
                }
                else if (prop.PropertyType == typeof(decimal))
                {
                    field.Type = "double";
                    field.Default = 0.0;

                    continue;
                }
                else if (prop.PropertyType == typeof(float))
                {
                    field.Type = "double";
                    field.Default = 0.0;

                    continue;
                }
                //Checking if is nullable
                else if (Nullable.GetUnderlyingType(prop.PropertyType) == typeof(int?)
                      || Nullable.GetUnderlyingType(prop.PropertyType) == typeof(double?)
                      || Nullable.GetUnderlyingType(prop.PropertyType) == typeof(decimal?)
                      || Nullable.GetUnderlyingType(prop.PropertyType) == typeof(float?)
                      || Nullable.GetUnderlyingType(prop.PropertyType) == typeof(long?)
                      || Nullable.GetUnderlyingType(prop.PropertyType) == typeof(bool?)
                      || Nullable.GetUnderlyingType(prop.PropertyType) == typeof(DateTimeOffset?)
                      || Nullable.GetUnderlyingType(prop.PropertyType) == typeof(DateTime?)
                      || Nullable.GetUnderlyingType(prop.PropertyType) == typeof(Guid?)
                      || prop.PropertyType.IsGenericType &&
                            prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    Type u = Nullable.GetUnderlyingType(prop.PropertyType);
                    if (u != null && u.IsEnum)
                    {
                        //TODO
                        field.Type = new string[]
                        {
                            "string",
                            "int"
                        };

                        field.Default = null;

                        continue;
                    }
                    else if (prop.PropertyType == typeof(int?))
                    {
                        field.Type = new string[]
                        {
                            "null",
                            "int"
                        };

                        field.Default = null;

                        continue;
                    }
                    else if (prop.PropertyType == typeof(long?))
                    {
                        field.Type = new string[]
                        {
                            "null",
                            "long"
                        };

                        field.Default = null;

                        continue;
                    }
                    else if (prop.PropertyType == typeof(DateTime?))
                    {
                        field.Type = new string[]
                        {
                            "null",
                            "string"
                        };

                        field.Default = null;

                        continue;
                    }
                    else if (prop.PropertyType == typeof(DateTimeOffset?))
                    {
                        field.Type = new string[]
                        {
                            "null",
                            "string"
                        };

                        field.Default = null;

                        continue;
                    }
                    else if (prop.PropertyType == typeof(Guid?))
                    {
                        field.Type = new string[]
                        {
                            "null",
                            "string"
                        };

                        field.Default = null;

                        continue;
                    }
                    else if (prop.PropertyType == typeof(bool?))
                    {
                        field.Type = new string[]
                        {
                        "null",
                        "boolean"
                        };

                        field.Default = null;

                        continue;
                    }
                    else if (prop.PropertyType == typeof(double?))
                    {
                        field.Type = new string[]
                        {
                        "null",
                        "double"
                        };

                        field.Default = null;

                        continue;
                    }
                    else if (prop.PropertyType == typeof(decimal?))
                    {
                        field.Type = new string[]
                        {
                        "null",
                        "double"
                        };

                        field.Default = null;

                        continue;
                    }
                    else if (prop.PropertyType == typeof(float?))
                    {
                        field.Type = new string[]
                        {
                        "null",
                        "double"
                        };

                        field.Default = null;

                        continue;
                    }
                    else if (prop.PropertyType.IsClass && prop.PropertyType != typeof(string))
                    {
                        string propTypeFullName = prop.PropertyType.FullName;
                        string propTypeFullName21 = prop.PropertyType.Namespace;

                        var selected = dublicates.FirstOrDefault(x => x.Namespace + "." + x.Name == propTypeFullName);

                        if (selected == null)
                        {
                            AvroSchema newAvro = new AvroSchema();

                            field.Type = new dynamic[]
                            {
                             "null",
                             newAvro,
                            };

                            dublicates.Add(newAvro);

                            newAvro.Type = "record";
                            newAvro.Name = prop.PropertyType.Name.TrimEnd(new char[] { '[', ']' });
                            newAvro.Namespace = prop.PropertyType.Namespace;

                            GetAvroPropertyCollection(prop.PropertyType, dublicates, newAvro, ignoreSchemaDefault);
                        }
                        else
                        {
                            field.Type = new dynamic[]
                           {
                            "null",
                            $"{prop.PropertyType.Namespace}.{prop.PropertyType.Name}"
                           };

                            field.Default = null;
                            continue;
                        }
                    }
                }
                else if (prop.PropertyType.IsClass && prop.PropertyType != typeof(string))
                {
                    string propTypeFullName = prop.PropertyType.FullName;

                    var selected = dublicates.FirstOrDefault(x => x.Namespace + "." + x.Name == propTypeFullName);

                    if (selected == null)
                    {
                        AvroSchema newAvro = new AvroSchema();

                        field.Type = new dynamic[]
                        {
                         "null",
                         newAvro,
                        };

                        dublicates.Add(newAvro);

                        newAvro.Type = "record";
                        newAvro.Name = prop.PropertyType.Name.TrimEnd(new char[] { '[', ']' });
                        newAvro.Namespace = prop.PropertyType.Namespace;

                        GetAvroPropertyCollection(prop.PropertyType, dublicates, newAvro, ignoreSchemaDefault);
                    }
                    else
                    {
                        field.Type = new dynamic[]
                       {
                        "null",
                        $"{prop.PropertyType.Namespace}.{prop.PropertyType.Name}"
                       };

                        field.Default = null;
                        continue;
                    }
                }
            }

            return schema;
        }

        /// <summary>
        /// Check type and set in some cases avro field type
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        private AvroField SetAvroFieldForCollections(Type prop, dynamic field)
        {
            if (prop == typeof(int))
            {
                field.Type = "int";
                field.Default = 0;
            }
            if (prop == typeof(long))
            {
                field.Type = "long";
                field.Default = 0;
            }
            else if (prop == typeof(string))
            {
                field.Type = new string[]
                {
                  "null",
                  "string"
                };

                field.Default = null;
            }
            else if (prop == typeof(DateTime))
            {
                field.Type = new string[]
                {
                  "null",
                  "string"
                };

                field.Default = null;
            }
            else if (prop == typeof(DateTimeOffset))
            {
                field.Type = new string[]
                {
                   "null",
                   "string"
                };

                field.Default = null;
            }
            else if (prop.IsEnum)
            {
                field.Type = new string[]
                 {
                   "null",
                   "string"
                 };

                field.Default = null;
            }
            else if (prop == typeof(Guid))
            {
                field.Type = new string[]
                {
                  "null",
                  "string"
                };

                field.Default = null;
            }
            else if (prop == typeof(bool))
            {
                field.Type = "boolean";

                field.Default = false;
            }
            else if (prop == typeof(double))
            {
                field.Type = "double";
                field.Default = 0.0;
            }
            else if (prop == typeof(decimal))
            {
                field.Type = "double";
                field.Default = 0.0;
            }
            else if (prop == typeof(float))
            {
                field.Type = "double";
                field.Default = 0.0;
            }
            //Checking if is nullable
            else if (Nullable.GetUnderlyingType(prop) == typeof(int?)
                  || Nullable.GetUnderlyingType(prop) == typeof(double?)
                  || Nullable.GetUnderlyingType(prop) == typeof(decimal?)
                  || Nullable.GetUnderlyingType(prop) == typeof(float?)
                  || Nullable.GetUnderlyingType(prop) == typeof(long?)
                  || Nullable.GetUnderlyingType(prop) == typeof(bool?)
                  || Nullable.GetUnderlyingType(prop) == typeof(DateTimeOffset?)
                  || Nullable.GetUnderlyingType(prop) == typeof(DateTime?)
                  || Nullable.GetUnderlyingType(prop) == typeof(Guid?)
                  || prop.IsGenericType &&
                        prop.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                Type u = Nullable.GetUnderlyingType(prop);
                if (u != null && u.IsEnum)
                {
                    //TODO
                    field.Type = new string[]
                    {
                      "string",
                      "int"
                    };

                    field.Default = null;

                }
                else if (prop == typeof(int?))
                {
                    field.Type = new string[]
                    {
                      "null",
                      "int"
                    };

                    field.Default = null;
                }
                else if (prop == typeof(long?))
                {
                    field.Type = new string[]
                    {
                      "null",
                      "long"
                    };

                    field.Default = null;
                }
                else if (prop == typeof(DateTime?))
                {
                    field.Type = new string[]
                    {
                      "null",
                      "string"
                    };

                    field.Default = null;
                }
                else if (prop == typeof(DateTimeOffset?))
                {
                    field.Type = new string[]
                    {
                      "null",
                      "string"
                    };

                    field.Default = null;
                }
                else if (prop == typeof(Guid?))
                {
                    field.Type = new string[]
                    {
                      "null",
                      "string"
                    };

                    field.Default = null;
                }
                else if (prop == typeof(bool?))
                {
                    field.Type = "boolean";

                    field.Default = false;
                }
                else if (prop == typeof(double?))
                {
                    field.Type = new string[]
                    {
                      "null",
                      "double"
                    };

                    field.Default = null;
                }
                else if (prop == typeof(decimal?))
                {
                    field.Type = new string[]
                    {
                      "null",
                      "double"
                    };

                    field.Default = null;
                }
                else if (prop == typeof(float?))
                {
                    field.Type = new string[]
                    {
                      "null",
                      "double"
                    };

                    field.Default = null;
                }

            }

            return field;
        }


        #region old methods
        //public AvroSchema GetAvroPropertyCollectionUrban(Type type, AvroSchema schema, List<AvroSchema> dublicates, ClassGenerateSettings settings = null)
        //{
        //    string typeName = type.Name.TrimEnd(new char[] { '[', ']' });
        //    //string propFullName = type.FullName.TrimEnd(new char[] { '[', ']' });           

        //    schema.Type = "record";
        //    schema.Name = typeName;
        //    schema.Namespace = type.Namespace;

        //    schema.Fields = new List<AvroField>();

        //    PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);

        //    return GetAvroSchema(properties, schema, dublicates);
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

        /// <summary>
        /// Method for file write in specific directory
        /// </summary>
        /// <param name="file"> actual file (class or schema will be represented as string)</param>
        /// <param name="fileName"> desired fileName (class name or schema name)</param>
        /// <param name="fileType"> wanted file extension as .cs, .avsc, .json etc</param>
        /// <param name="directory"> where you want to save it (possible to have nested folders)</param>
        //public void WriteFile(string file, string fileName, string fileExtension, params string[] directories)
        #endregion
    }
}
