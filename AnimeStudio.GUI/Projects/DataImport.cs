using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Vortice.Win32;

namespace AnimeStudio.GUI.Projects
{
    internal class DataImport
    {
        private static List<TreeNodeDTO> treeNodes = new();
        private static AssetsManager internalAssetsManager = new AssetsManager() { Silent = true, ResolveDependencies = false };

        public async static Task ImportFolder(string folderPath)
        {
            Console.WriteLine($"Importing folder: {folderPath}");

            // get files in the folder
            var files = System.IO.Directory.GetFiles(folderPath);

            //ProcessFile(files[0]);
            //foreach (var file in files)
            //{
            //    await Task.Run(() => ProcessFile(file));
            //}

            //await Task.WhenAll(files.Select(file => Task.Run(() => ProcessFile(file))));


            //string filename = Path.Combine($"TESTING.json");
            //using StreamWriter tempfile = File.CreateText(filename);
            //var serializer = new JsonSerializer() { Formatting = Newtonsoft.Json.Formatting.Indented };
            //serializer.Converters.Add(new StringEnumConverter());

            //Logger.Info($"treNode size : {treeNodes}");

            //serializer.Serialize(tempfile, treeNodes);




            // V2
            
            (var nodesWriter, var nodesSerializer) = CreateJsonWriter("nodes");
            (var typesWriter, var typesSerializer) = CreateJsonWriter("types");

            var existingNames = new HashSet<string>();

            
            nodesWriter.WriteStartArray();
            typesWriter.WriteStartArray();

            //foreach (var file in files)
            //{
            //    Logger.Info($"processing file {file}");
            //}

            foreach (var file in files)
            {
                (var dtoList, var types) = await Task.Run(() => ProcessFile(file));
                //var dtoList = await Task.Run(() => ProcessFile(file));
                //Task.Run(() => ProcessFile(file));

                //Logger.Info($"file {file} has {dtoList.Count} nodes");
                Logger.Info($"processing file {file} with {dtoList.Count} nodes and {types.Count} types");

                foreach (var node in dtoList)
                {
                    Logger.Info($"processing node {node.Text}");
                    if (existingNames.Add(node.Text))
                    {
                        Logger.Info("i wrote");
                        nodesSerializer.Serialize(nodesWriter, node);
                    }
                }

                typesSerializer.Serialize(typesWriter, types);

                nodesWriter.Flush();
                typesWriter.Flush();
            }

            nodesWriter.WriteEndArray();
            typesWriter.WriteEndArray();

            Logger.Info("done !");
        }

        private static (JsonTextWriter, JsonSerializer) CreateJsonWriter(string name)
        {
            string filename = Path.Combine($"{name}.json");
            var stream = File.CreateText(filename);
            var writer = new JsonTextWriter(stream) { Formatting = Formatting.Indented };
            var serializer = new JsonSerializer();
            serializer.Converters.Add(new StringEnumConverter());
            return (writer, serializer);
        }

        public async static Task<(List<TreeNodeDTO>, Dictionary<string, SortedDictionary<int, TypeTreeItem>>)> ProcessFile(string filePath)
        {
            Console.WriteLine($"Processing file: {filePath}");
            
            
            internalAssetsManager.Game = Studio.Game;

            internalAssetsManager.LoadFiles(filePath);
            //assetsManager.

            if (internalAssetsManager.assetsFileList.Count == 0)
            {
                Logger.Info("nothing found");
                return ([], []);
            }

            (var productName, var treeNodeCollection) = await Task.Run(() => Studio.BuildAssetData(internalAssetsManager));
            var typeMap = await Task.Run(() => Studio.BuildClassStructure(internalAssetsManager));

            //Logger.Info($"{typeMap.Values.FirstOrDefault().Values.FirstOrDefault().ToString()}");

            var dtoList = new Projects.TreeNodeJSON().ConvertAll(treeNodeCollection);
            Logger.Info($"{treeNodeCollection.Count}");

            //foreach (TreeNodeDTO node in dtoList)
            //{
            //    if (!treeNodes.Any(n => n.Text == node.Text))
            //        treeNodes.Add(node);
            //}
            //treeNodes.AddRange(dtoList);

            internalAssetsManager.Clear();

            Logger.Info($"post clear afl count ${internalAssetsManager.assetsFileList.Count}");

            return (dtoList, typeMap);
        }
    }
}
