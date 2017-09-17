using CAD.FEA.WebApp.Controllers.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.SessionState;
//using System.Configuration;
//using System.Diagnostics;
using System.Configuration;
using CADFEA.AzureStorage;
using Microsoft.WindowsAzure;
//using QuantumConcepts.Formats.StereoLithography;
using System.Diagnostics;
using CADFEA.WebApi.Controllers;
using CAD.FEA.Share.Meshing.Tetgen;

namespace CAD.FEA.WebApp.Controllers
{
    [SessionState(SessionStateBehavior.ReadOnly)]
    [Authorize]
    public class ImportController : CommonController
    {
        [HttpPost]
        public ActionResult Index()
        {
            try
            {
                SessionData session = GetSessionData();

                string fName = Request.Params["uploadfile"];
                if (String.IsNullOrEmpty(fName) ||
                    Request.InputStream == null ||
                    Request.InputStream.Length == 0)
                {
                    throw new Exception("Upload filename not set");
                }

                string destPath = Path.Combine(session.SessionFolder, fName);

                using (Stream receiveStream = Request.InputStream)
                {
                    var buf = new byte[receiveStream.Length];
                    int readCnt = receiveStream.Read(buf, 0, (int)receiveStream.Length);
                    if (readCnt != receiveStream.Length)
                    {
                        throw new Exception("Can't read upload file");
                    }

                    using (var fileStream = System.IO.File.Create(destPath))
                    {
                        fileStream.Write(buf, 0, readCnt);
                    }
                }

                session.FilePathOrig = destPath;

                return JsonSuccess;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return GetJsonFail(ex);
            }
        }



        #region Commented
        //[HttpPost]
        //public ActionResult Convert()
        //{
        //    // var ra = new devDept.Eyeshot.Translators.ReadOBJ("", true);
        //    ServerTask servTask = null;

        //    try
        //    {
        //        SessionData session = GetSessionData();
        //        if (session == null)
        //        {
        //            throw new Exception("Session not avalaible");
        //        }
        //        if (String.IsNullOrEmpty(session.FilePathOrig))
        //        {
        //            throw new Exception("Model filename not set");
        //        }
        //        if (String.IsNullOrEmpty(session.FilePathOrig))
        //        {
        //            throw new Exception("Model not set");
        //        }

        //        //Import to scene
        //        //STLDocument stlDocument = STLDocument.Open(fileName);
        //        //stlDocument.SaveAsBinary(fileName);


        //        servTask = session.TaskManager.CreateSingleTask("Convert model...", "Import.Convert", session.UserId);

        //        string pathOrig = session.FilePathOrig;

        //        // Convert to SAT
        //        string inExt = Path.GetExtension(pathOrig);
        //        inExt = inExt.Replace(".", "");
        //        string pathSat = "";
        //        if ("sat" == inExt)
        //        {
        //            pathSat = pathOrig;


        //            // Rename filename
        //            //
        //            string destPathSat = session.GetModelPath();
        //            if (System.IO.File.Exists(destPathSat))
        //            {
        //                System.IO.File.Delete(destPathSat);
        //            }
        //            System.IO.File.Move(pathSat, destPathSat);
        //        }
        //        else
        //        {
        //            //try
        //            //{


        //            var inputContainerName = ConfigurationManager.AppSettings["InputContainer"];
        //            var outputContainer = ConfigurationManager.AppSettings["OutputContainer"];

        //            var storageCnn = CloudConfigurationManager.GetSetting("StorageConnectionString");
        //            var inStorage = new Storage(storageCnn, inputContainerName);
        //            var outStorage = new Storage(storageCnn, outputContainer);

        //            var stlFileName = Path.GetFileName(pathOrig);

        //            if (inStorage.FileExists(stlFileName))
        //            {
        //                inStorage.DeleteFile(stlFileName);
        //            }

        //            inStorage.CreateFile(stlFileName, System.IO.File.ReadAllBytes(pathOrig));

        //            //var url = ConfigurationManager.AppSettings["ConvertWebJobTriggerUrl"];
        //            //var userName = ConfigurationManager.AppSettings["ConvertWebJobUserName"];
        //            //var password = ConfigurationManager.AppSettings["ConvertWebJobPassword"];


        //            //HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
        //            //request.Method = "POST";
        //            //var byteArray = Encoding.ASCII.GetBytes($"{userName}:{password}"); //we could find user name and password in Azure web app publish profile 
        //            //request.Headers.Add("Authorization", "Basic " + System.Convert.ToBase64String(byteArray));
        //            //request.ContentLength = 0;
        //            //try
        //            //{
        //            //    var response = (HttpWebResponse)request.GetResponse();
        //            //}
        //            //catch (Exception ex)
        //            //{
        //            //    Logger.Error(ex);

        //            //}
        //            //}
        //            //catch (Exception)
        //            //{


        //            //}

        //            Logger.Info("Using STLDocument to convert");
        //            //STLDocument stlDocument = STLDocument.Open(pathOrig);
        //            //stlDocument.SaveAsBinary(pathOrig);
        //            var satFileName = pathOrig;//ConvertClient.Instance.ConvertToStl(stlFileName);

        //            Logger.Info($"satFileName {satFileName}");

        //            // Logger.Info("Calling CadLook.Convert");

        //            // pathSat = CadLook.Convert(pathOrig, "sat");

        //            //  System.IO.File.WriteAllBytes(pathSat, outStorage.GetFile(pathSat));

        //            //if (!System.IO.File.Exists(pathSat))
        //            //{
        //            //    return GetJsonFail(new Exception($"Path not exist. Conversion failed. {pathSat}"));
        //            //}

        //            //Logger.Info($"pathSat {pathSat}");

        //            string destPathSat = session.GetModelPath();

        //            Logger.Info($"destPathSat  {destPathSat}");

        //            if (System.IO.File.Exists(destPathSat))
        //            {
        //                System.IO.File.Delete(destPathSat);
        //            }

        //            Logger.Info($"Writing  file {destPathSat}");

        //            System.IO.File.WriteAllBytes(destPathSat, outStorage.GetFile(pathOrig));

        //            Logger.Info($"Writing  file success {destPathSat}");

        //            //   Logger.Info($"Moving file");
        //            // Logger.Info($"Moving file");
        //            // System.IO.File.Move(pathSat, destPathSat);

        //            //  Logger.Info($"File moved.");

        //        }
        //        return JsonSuccess;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error(ex);
        //        return GetJsonFail(ex);
        //    }
        //    finally
        //    {
        //        if (servTask != null)
        //        {
        //            servTask.Stop();
        //        }
        //    }
        //} 
        #endregion


        public bool ValidAssimpModel(string extension)
        {
            List<string> ValidAssimpExtension = new List<string>()
            { "fbx", "dae","gltf","glb","blend","3ds","ase","obj","ifc","xgl","zgl","ply","dxf","lwo","lws","lxo", "x",
            "ac","ms3d","cob","scn","ogex","x3d","3mf","stl"};

            foreach (var ext in ValidAssimpExtension)
            {
                if (extension == ext)
                    return true;
            }
            return false;

        }


        [HttpPost]
        public ActionResult Convert()
        {
            // var ra = new devDept.Eyeshot.Translators.ReadOBJ("", true);
            ServerTask servTask = null;

            try
            {
                SessionData session = GetSessionData();
                if (session == null)
                {
                    throw new Exception("Session not avalaible");
                }
                if (String.IsNullOrEmpty(session.FilePathOrig))
                {
                    throw new Exception("Model filename not set");
                }
                if (String.IsNullOrEmpty(session.FilePathOrig))
                {
                    throw new Exception("Model not set");
                }

                //Import to scene
                //STLDocument stlDocument = STLDocument.Open(fileName);
                //stlDocument.SaveAsBinary(fileName);


                servTask = session.TaskManager.CreateSingleTask("Convert model...", "Import.Convert", session.UserId);

                string pathOrig = session.FilePathOrig;

                // Convert to SAT
                string inExt = Path.GetExtension(pathOrig);
                inExt = inExt.Replace(".", "");
                string pathSat = "";
                if ("sat" == inExt)
                {
                    pathSat = pathOrig;


                    // Rename filename
                    //
                    string destPathSat = session.GetModelPath();
                    if (System.IO.File.Exists(destPathSat))
                    {
                        System.IO.File.Delete(destPathSat);
                    }
                    System.IO.File.Move(pathSat, destPathSat);
                }
                else if (ValidAssimpModel(inExt)) //file is supported assimp model
                {
                    //convert assimp to ply and mesh with tetgen


                    //string fileToCopy = "filename[0]";//filename[0];was casting.stl
                    //string fileExtension = session.FilePathOrig.Split('.')[1];
                    //string pathToExe = @"C:\Program Files\CADlook\CADlook64_V19.0\CADlook64MT.exe";//Server.MapPath("~/Assimp/assimp.exe"); ;
                    //string pathToDirectory = @"C:\Program Files\CADlook\CADlook64_V19.0"; ;//Server.MapPath("~/Assimp/");


                    string assimpfileToCopy = "filename[0]";//filename[0];was casting.stl
                    //string fileToCopy = filename[0];
                    // string pathToExe = Server.MapPath("~/AssimpToJson/assimp2json.exe");
                    string pathToExe = Server.MapPath("~/Assimp/assimp.exe");
                    // string pathToDirectory = Server.MapPath("~/AssimpToJson/");
                    //string pathToDirectory = Server.MapPath("~/Assimp/");

                    string pathToDirectory = session.GetModelPath();

                    var vproc = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = pathToExe,
                            Arguments = "export" + " " + assimpfileToCopy + " " + assimpfileToCopy.Split('.')[0] + ".ply",
                            WorkingDirectory = pathToDirectory
                        }
                    };

                    vproc.Start();


                    //if (fileExtension.ToLower() == "stl")
                    //{


                    //    //#region Clear Directory
                    //    //string[] filesInDirtectory = Directory.GetFiles(pathToDirectory);
                    //    //foreach (var file in filesInDirtectory)
                    //    //{
                    //    //    var fileName = new FileInfo(file).Name;

                    //    //    if (fileName.ToLower() != "assimp.dll" && fileName.ToLower() != "assimp.exe" && fileName.ToLower() != "assimp_viewer.exe")
                    //    //    {
                    //    //        System.IO.File.Delete(file);
                    //    //    }
                    //    //}
                    //    //#endregion

                    //    #region Copy File To Direcoty

                    //    try
                    //    {
                    //        System.IO.File.Copy(session.FilePathOrig, Path.Combine(pathToDirectory, fileToCopy), true);
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        throw;
                    //    }
                    //    #endregion
                    //    var proc = new Process
                    //    {
                    //        StartInfo = new ProcessStartInfo
                    //        {
                    //            FileName = pathToExe,
                    //            Arguments = " " + "-i" + fileToCopy + " " + "-o" + fileToCopy.Split('.')[0] + ".sat",
                    //            //string args = string.Format(" -log \"{0}\" -i \"{1}\" -o \"{2}\"", logfilePath, inFilePath, outFile);
                    //            WorkingDirectory = pathToDirectory,
                    //            WindowStyle = ProcessWindowStyle.Hidden
                    //        }
                    //    };

                    //    proc.Start();
                    //    proc.WaitForExit();



                    //}
                    //else 
                    //{

                    //    #region Clear Directory
                    //    string[] filesInDirtectory = Directory.GetFiles(pathToDirectory);
                    //    foreach (var file in filesInDirtectory)
                    //    {
                    //        var fileName = new FileInfo(file).Name;

                    //        if (fileName.ToLower() != "assimp.dll" && fileName.ToLower() != "assimp.exe" && fileName.ToLower() != "assimp_viewer.exe")
                    //        {
                    //            System.IO.File.Delete(file);
                    //        }
                    //    }
                    //    #endregion

                    //    #region Copy File To Direcoty

                    //    try
                    //    {
                    //        System.IO.File.Copy(session.FilePathOrig, Path.Combine(pathToDirectory, fileToCopy), true);
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        throw;
                    //    }
                    //    #endregion
                    //    var proc = new Process
                    //    {
                    //        StartInfo = new ProcessStartInfo
                    //        {
                    //            FileName = pathToExe,
                    //            Arguments = " " + "-i" + fileToCopy + " " + "-o" + fileToCopy.Split('.')[0] + ".sat",
                    //            //string args = string.Format(" -log \"{0}\" -i \"{1}\" -o \"{2}\"", logfilePath, inFilePath, outFile);
                    //            WorkingDirectory = pathToDirectory,
                    //            WindowStyle = ProcessWindowStyle.Hidden
                    //        }
                    //    };

                    //    proc.Start();
                    //    proc.WaitForExit();



                    //}


                    #region COMMENTED

                    //first convert to sat using cadllook





                    if (inExt == "stl") //convert to sat
                    {
                        #region Convert to solid geo and then mesh it using semish
                        #region Clear Directory
                        string[] filesInDirtectory = Directory.GetFiles(Server.MapPath("~/MeshLabServer/"));
                        foreach (var file in filesInDirtectory)
                        {
                            var fileName = new FileInfo(file).Name;

                            if (fileName.ToLower() == "temp.off")
                            {
                                System.IO.File.Delete(file);
                            }
                        }
                        #endregion

                        #region Copy File To Direcoty

                        try
                        {
                            System.IO.File.Copy(session.FilePathOrig, Path.Combine(Server.MapPath("~/MeshLabServer/"), "colored.stl"), true);
                        }
                        catch (Exception ex)
                        {
                            throw;
                        }
                        #endregion
                        string fileToCopy = Path.Combine(Server.MapPath("~/MeshLabServer/"), "colored.stl");
                        #region ConvertTo_.Off
                        if (true)
                        {
                            var proc = new Process
                            {
                                StartInfo = new ProcessStartInfo
                                {
                                    FileName = Server.MapPath("~/MeshLabServer/meshlabserver.exe"),
                                    Arguments = "-i" + " " + fileToCopy + " " + "-o" + "temp.off",
                                    WorkingDirectory = Server.MapPath("~/MeshLabServer/"),
                                    WindowStyle = ProcessWindowStyle.Hidden
                                }
                            };

                            proc.Start();
                        }
                        #endregion
                        #endregion

                        //}
                        //else
                        //{

                        //} 
                        #endregion

                    }
                    else
                        //{
                        //try
                        //{


                        //var inputContainerName = ConfigurationManager.AppSettings["InputContainer"];
                        //var outputContainer = ConfigurationManager.AppSettings["OutputContainer"];

                        //var storageCnn = CloudConfigurationManager.GetSetting("StorageConnectionString");
                        //var inStorage = new Storage(storageCnn, inputContainerName);
                        //var outStorage = new Storage(storageCnn, outputContainer);

                        //var stlFileName = Path.GetFileName(pathOrig);

                        //if (inStorage.FileExists(stlFileName))
                        //{
                        //    inStorage.DeleteFile(stlFileName);
                        //}

                        //inStorage.CreateFile(stlFileName, System.IO.File.ReadAllBytes(pathOrig));

                        //var url = ConfigurationManager.AppSettings["ConvertWebJobTriggerUrl"];
                        //var userName = ConfigurationManager.AppSettings["ConvertWebJobUserName"];
                        //var password = ConfigurationManager.AppSettings["ConvertWebJobPassword"];


                        //HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                        //request.Method = "POST";
                        //var byteArray = Encoding.ASCII.GetBytes($"{userName}:{password}"); //we could find user name and password in Azure web app publish profile 
                        //request.Headers.Add("Authorization", "Basic " + System.Convert.ToBase64String(byteArray));
                        //request.ContentLength = 0;
                        //try
                        //{
                        //    var response = (HttpWebResponse)request.GetResponse();
                        //}
                        //catch (Exception ex)
                        //{
                        //    Logger.Error(ex);

                        //}
                        //}
                        //catch (Exception)
                        //{


                        //}

                        Logger.Info("Using STLDocument to convert");
                    //STLDocument stlDocument = STLDocument.Open(pathOrig);
                    //stlDocument.SaveAsBinary(pathOrig);
                    var satFileName = pathOrig;//ConvertClient.Instance.ConvertToStl(stlFileName);

                    Logger.Info($"satFileName {satFileName}");

                    // Logger.Info("Calling CadLook.Convert");

                    // pathSat = CadLook.Convert(pathOrig, "sat");

                    //  System.IO.File.WriteAllBytes(pathSat, outStorage.GetFile(pathSat));

                    //if (!System.IO.File.Exists(pathSat))
                    //{
                    //    return GetJsonFail(new Exception($"Path not exist. Conversion failed. {pathSat}"));
                    //}

                    //Logger.Info($"pathSat {pathSat}");

                    string destPathSat = session.GetModelPath();

                    Logger.Info($"destPathSat  {destPathSat}");

                    if (System.IO.File.Exists(destPathSat))
                    {
                        System.IO.File.Delete(destPathSat);
                    }

                    Logger.Info($"Writing  file {destPathSat}");

                    //System.IO.File.WriteAllBytes(destPathSat, outStorage.GetFile(pathOrig));

                    Logger.Info($"Writing  file success {destPathSat}");

                    //   Logger.Info($"Moving file");
                    // Logger.Info($"Moving file");
                    // System.IO.File.Move(pathSat, destPathSat);

                    //  Logger.Info($"File moved.");

                }
                return JsonSuccess;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return GetJsonFail(ex);
            }
            finally
            {
                if (servTask != null)
                {
                    servTask.Stop();
                }
            }
        }






        string fileName = "";
        [HttpPost]
        public ActionResult ImportModelFromStorage()
        {
            ServerTask servTask = null;
            try
            {
                SessionData session = GetSessionData();

                servTask = session.TaskManager.CreateTask("Load Model From Storage ...");

                string file = Request.Params["file"];
                if (String.IsNullOrEmpty(file))
                {
                    throw new ArgumentNullException("Argument 'file' is empty");
                }


                fileName = Path.GetFileName(file);
                string destPath = Path.Combine(session.SessionFolder, fileName);

                var data = session.AzureStorage.GetFile(file);
                if (data == null || data.Length == 0)
                {
                    throw new UserException("File not found in storage");
                }
                System.IO.File.WriteAllBytes(destPath, data);

                session.FilePathOrig = destPath;

                // Start converting
                //Convert();

                return JsonSuccess;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return GetJsonFail(ex);
            }
            finally
            {
                if (servTask != null)
                {
                    servTask.Stop();
                }
            }
        }


        [HttpPost]
        public ActionResult ConvertMesh()
        {
            ServerTask servTask = null;

            try
            {
                SessionData session = GetSessionData();
                if (session == null)
                {
                    throw new Exception("Session not avalaible");
                }
                if (String.IsNullOrEmpty(session.FilePathOrig))
                {
                    throw new Exception("Model filename not set");
                }
                if (String.IsNullOrEmpty(session.FilePathOrig))
                {
                    throw new Exception("Model not set");
                }

                servTask = session.TaskManager.CreateSingleTask("Convert model...", "Import.Convert", session.UserId);

                string pathOrig = session.FilePathOrig;

                // Convert to SAT
                string inExt = Path.GetExtension(pathOrig);
                inExt = inExt.Replace(".", "");
                string pathSat = pathOrig;


                // Rename filename
                //
                string destPathSat = session.GetModelPathDiff(inExt);
                if (System.IO.File.Exists(destPathSat))
                {
                    System.IO.File.Delete(destPathSat);
                }
                System.IO.File.Move(pathSat, destPathSat);

                return JsonSuccess;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return GetJsonFail(ex);
            }
            finally
            {
                if (servTask != null)
                {
                    servTask.Stop();
                }
            }
        }


        #region STL Button (Import and Render Stl functionality)
        public bool ImportSTL(string[] filename)
        {
            SessionData session = GetSessionData();

            if (filename.Count() > 0 && session != null)
            {
                string fileToCopy = filename[0];
                string fileExtension = session.FilePathOrig.Split('.')[1];
                string pathToExe = Server.MapPath("~/Assimp/assimp.exe"); ;
                string pathToDirectory = Server.MapPath("~/Assimp/");


                #region Clear Directory
                string[] filesInDirtectory = Directory.GetFiles(pathToDirectory);
                foreach (var file in filesInDirtectory)
                {
                    var fileName = new FileInfo(file).Name;

                    if (fileName.ToLower() != "assimp.dll" && fileName.ToLower() != "assimp.exe" && fileName.ToLower() != "assimp_viewer.exe")
                    {
                        System.IO.File.Delete(file);
                    }
                }
                #endregion

                #region Copy File To Direcoty

                try
                {
                    System.IO.File.Copy(session.FilePathOrig, Path.Combine(pathToDirectory, fileToCopy), true);
                }
                catch (Exception ex)
                {
                    throw;
                }
                #endregion


                if (fileExtension.ToLower() != "stl" && ValidAssimpModel(fileExtension)) //file is assimp supported
                {
                    var proc = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = pathToExe,
                            Arguments = "export" + " " + fileToCopy + " " + fileToCopy.Split('.')[0] + ".stl",
                            WorkingDirectory = pathToDirectory,
                            WindowStyle = ProcessWindowStyle.Hidden
                        }
                    };

                    proc.Start();
                    proc.WaitForExit();
                    TempData["FiletoRender"] = fileToCopy.Split('.')[0] + ".stl";
                    return true;
                }

                else if (fileExtension.ToLower() == "stl") // file is stl
                {
                    TempData["FiletoRender"] = fileToCopy;
                    return true;
                }
                else // it is invalid file type
                {
                    return false;
                }
            }
            ViewBag.IsStl = "STL";

            return true;
        }
        public ActionResult RenderSTL()
        {
            return View();
        }
        #endregion



        [HttpPost]
        public ActionResult RenderPly(string[] fileArray)
        {


            // var ra = new devDept.Eyeshot.Translators.ReadOBJ("", true);
            ServerTask servTask = null;

            try
            {
                SessionData session = GetSessionData();
                if (session == null)
                {
                    throw new Exception("Session not avalaible");
                }
                if (String.IsNullOrEmpty(session.FilePathOrig))
                {
                    throw new Exception("Model filename not set");
                }
                if (String.IsNullOrEmpty(session.FilePathOrig))
                {
                    throw new Exception("Model not set");
                }

                //Import to scene
                //STLDocument stlDocument = STLDocument.Open(fileName);
                //stlDocument.SaveAsBinary(fileName);


                servTask = session.TaskManager.CreateSingleTask("Convert model...", "Import.Convert", session.UserId);

                string pathOrig = session.FilePathOrig;

                // Convert to PLY
                string inExt = Path.GetExtension(pathOrig);
                inExt = inExt.Replace(".", "");
                string pathSat = "";
                if ("ply" == inExt)
                {
                    pathSat = pathOrig;


                    // Rename filename
                    //
                    string destPathSat = session.GetModelPath();
                    if (System.IO.File.Exists(destPathSat))
                    {
                        System.IO.File.Delete(destPathSat);
                    }
                    System.IO.File.Move(pathSat, destPathSat);
                }
                else
                {
                    //try
                    //{


                    var inputContainerName = ConfigurationManager.AppSettings["InputContainer"];
                    var outputContainer = ConfigurationManager.AppSettings["OutputContainer"];

                    var storageCnn = CloudConfigurationManager.GetSetting("StorageConnectionString");
                    var inStorage = new Storage(storageCnn, inputContainerName);
                    var outStorage = new Storage(storageCnn, outputContainer);

                    var stlFileName = Path.GetFileName(pathOrig);

                    if (inStorage.FileExists(stlFileName))
                    {
                        inStorage.DeleteFile(stlFileName);
                    }

                    inStorage.CreateFile(stlFileName, System.IO.File.ReadAllBytes(pathOrig));

                    //var url = ConfigurationManager.AppSettings["ConvertWebJobTriggerUrl"];
                    //var userName = ConfigurationManager.AppSettings["ConvertWebJobUserName"];
                    //var password = ConfigurationManager.AppSettings["ConvertWebJobPassword"];


                    //HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                    //request.Method = "POST";
                    //var byteArray = Encoding.ASCII.GetBytes($"{userName}:{password}"); //we could find user name and password in Azure web app publish profile 
                    //request.Headers.Add("Authorization", "Basic " + System.Convert.ToBase64String(byteArray));
                    //request.ContentLength = 0;
                    //try
                    //{
                    //    var response = (HttpWebResponse)request.GetResponse();
                    //}
                    //catch (Exception ex)
                    //{
                    //    Logger.Error(ex);

                    //}
                    //}
                    //catch (Exception)
                    //{


                    //}

                    Logger.Info("Using STLDocument to convert");
                    //STLDocument stlDocument = STLDocument.Open(pathOrig);
                    //stlDocument.SaveAsBinary(pathOrig);
                    var satFileName = pathOrig;//ConvertClient.Instance.ConvertToStl(stlFileName);

                    Logger.Info($"satFileName {satFileName}");

                    // Logger.Info("Calling CadLook.Convert");

                    // pathSat = CadLook.Convert(pathOrig, "sat");

                    //  System.IO.File.WriteAllBytes(pathSat, outStorage.GetFile(pathSat));

                    //if (!System.IO.File.Exists(pathSat))
                    //{
                    //    return GetJsonFail(new Exception($"Path not exist. Conversion failed. {pathSat}"));
                    //}

                    //Logger.Info($"pathSat {pathSat}");

                    string destPathSat = session.GetModelPath();

                    Logger.Info($"destPathSat  {destPathSat}");

                    if (System.IO.File.Exists(destPathSat))
                    {
                        System.IO.File.Delete(destPathSat);
                    }

                    Logger.Info($"Writing  file {destPathSat}");

                    System.IO.File.WriteAllBytes(destPathSat, outStorage.GetFile(pathOrig));

                    Logger.Info($"Writing  file success {destPathSat}");

                    //   Logger.Info($"Moving file");
                    // Logger.Info($"Moving file");
                    // System.IO.File.Move(pathSat, destPathSat);

                    //  Logger.Info($"File moved.");

                }
                return JsonSuccess;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return GetJsonFail(ex);
            }
            finally
            {
                if (servTask != null)
                {
                    servTask.Stop();
                }
            }

            #region Commented Old
            //SessionData session = GetSessionData();

            //if (fileArray.Count() > 0 && session != null)
            //{
            //    string fileToCopy = fileArray[0];
            //    // string pathToExe = Server.MapPath("~/AssimpToJson/assimp2json.exe");
            //    string pathToExe = Server.MapPath("~/Assimp/assimp.exe");
            //    // string pathToDirectory = Server.MapPath("~/AssimpToJson/");
            //    string pathToDirectory = Server.MapPath("~/Assimp/");

            //    #region Clear Directory
            //    string[] filesInDirtectory = Directory.GetFiles(pathToDirectory);
            //    foreach (var file in filesInDirtectory)
            //    {
            //        var fileName = new FileInfo(file).Name;
            //        // if (fileName.ToLower() != "assimp.dll" && fileName.ToLower() != "assimp2json.exe")
            //        if (fileName.ToLower() != "assimp.dll" && fileName.ToLower() != "assimp.exe" && fileName.ToLower() != "assimp_viewer.exe")
            //        {
            //            System.IO.File.Delete(file);
            //        }
            //    }
            //    #endregion

            //    #region Copy File To Direcoty

            //    try
            //    {
            //        System.IO.File.Copy(session.FilePathOrig, Path.Combine(pathToDirectory, fileToCopy), true);
            //    }
            //    catch (Exception ex)
            //    {
            //        throw;
            //    }
            //    #endregion

            //    #region Convert To .obj(collada)
            //    var proc = new Process
            //    {
            //        StartInfo = new ProcessStartInfo
            //        {
            //            FileName = pathToExe,
            //            Arguments = "export" + " " + fileToCopy + " " + fileToCopy.Split('.')[0] + ".obj",
            //            //  WorkingDirectory = Server.MapPath("~/AssimpToJson/")
            //            WorkingDirectory = pathToDirectory
            //        }
            //    };

            //    proc.Start();
            //    #endregion
            //} 
            #endregion

        }



        //ply was Obj
        public ActionResult CompleteImportGeo(string[] filename)
        {
            SessionData session = GetSessionData();

            if (filename.Count() > 0 && session != null)
            {
                string fileToCopy = filename[0];
                // string pathToExe = Server.MapPath("~/AssimpToJson/assimp2json.exe");
                string pathToExe = Server.MapPath("~/Assimp/assimp.exe");
                // string pathToDirectory = Server.MapPath("~/AssimpToJson/");
                string pathToDirectory = Server.MapPath("~/Assimp/");

                #region Clear Directory
                string[] filesInDirtectory = Directory.GetFiles(pathToDirectory);
                foreach (var file in filesInDirtectory)
                {
                    var fileName = new FileInfo(file).Name;
                    // if (fileName.ToLower() != "assimp.dll" && fileName.ToLower() != "assimp2json.exe")
                    if (fileName.ToLower() != "assimp.dll" && fileName.ToLower() != "assimp.exe" && fileName.ToLower() != "assimp_viewer.exe")
                    {
                        System.IO.File.Delete(file);
                    }
                }
                #endregion

                #region Copy File To Direcoty

                try
                {
                    System.IO.File.Copy(session.FilePathOrig, Path.Combine(pathToDirectory, fileToCopy), true);
                }
                catch (Exception ex)
                {
                    throw;
                }
                #endregion

                #region Convert To .ply
                var proc = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = pathToExe,
                        Arguments = "export" + " " + fileToCopy + " " + fileToCopy.Split('.')[0] + ".ply",
                        WorkingDirectory = pathToDirectory
                    }
                };

                proc.Start();
                #endregion
            }
            return JsonSuccess;
        }


        #region Old Load Geometry
        //[HttpPost]
        //public ActionResult CompleteImportGeo()
        //{
        //    //var ra = new devDept.Eyeshot.Translators.ReadOBJ("", true);
        //    ServerTask servTask = null;

        //    try
        //    {
        //        SessionData session = GetSessionData();
        //        if (session == null)

        //        {
        //            throw new Exception("Session not avalaible");
        //        }
        //        if (String.IsNullOrEmpty(session.FilePathOrig))
        //        {
        //            throw new Exception("Model filename not set");
        //        }
        //        if (String.IsNullOrEmpty(session.FilePathOrig))
        //        {
        //            throw new Exception("Model not set");
        //        }

        //        //Import to scene


        //        servTask = session.TaskManager.CreateSingleTask("Convert model...", "Import.Convert", session.UserId);

        //        string pathOrig = session.FilePathOrig;

        //        Assimp.Scene scene = new Assimp.Scene();
        //        // Load an existing 3D document
        //        scene.Open(pathOrig);
        //        var fileNameWithoutExt = Path.GetFileNameWithoutExtension(pathOrig);
        //        var stlFileName = string.Format("{0}.stl", fileNameWithoutExt);
        //        var pathSat = Path.Combine(session.SessionFolder, stlFileName);

        //        FileIOMode.Read.ToString(pathSat);
        //        //var fileNames = new List<string> {
        //        //pathOrig
        //        //};

        //        //ProcessFile("3.bat", fileNames);

        //        // Rename filename
        //        //
        //        string destPathSat = session.GetModelPathDiff("stl");
        //        if (System.IO.File.Exists(destPathSat))
        //        {
        //            System.IO.File.Delete(destPathSat);
        //        }
        //        System.IO.File.Move(pathSat, destPathSat);

        //        return JsonSuccess;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error(ex);
        //        return GetJsonFail(ex);
        //    }
        //    finally
        //    {
        //        if (servTask != null)
        //        {
        //            servTask.Stop();
        //        }
        //    }
        //} 
        #endregion

        [HttpPost]
        public ActionResult CompleteImportMesh()
        {
            //var ra = new devDept.Eyeshot.Translators.ReadOBJ("", true);
            //ServerTask servTask = null;

            try
            {
                SessionData session = GetSessionData();
                if (session == null)
                {
                    throw new Exception("Session not avalaible");
                }
                if (String.IsNullOrEmpty(session.FilePathOrig))
                {
                    throw new Exception("Model filename not set");
                }
                if (String.IsNullOrEmpty(session.FilePathOrig))
                {
                    throw new Exception("Model not set");
                }

                //Import to scene


                // servTask = session.TaskManager.CreateSingleTask("Convert model...", "Import.Convert", session.UserId);

                string pathOrig = session.FilePathOrig;
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(pathOrig);

                var fileNames = new List<string> {
                pathOrig
                //,
               // Path.Combine(session.SessionFolder, string.Format("{0}.smesh",fileNameWithoutExtension))
                };

                //  ProcessFile("2.bat", fileNames);

                //// Rename filename
                ////
                //string destPathSat = session.GetModelPath();
                //if (System.IO.File.Exists(destPathSat))
                //{
                //    System.IO.File.Delete(destPathSat);
                //}
                //System.IO.File.Move(pathSat, destPathSat);

                return JsonSuccess;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return GetJsonFail(ex);
            }
            finally
            {
                //if (servTask != null)
                //{
                //    servTask.Stop();
                //}
            }
        }

        //private void ProcessFile(string batFileName, List<string> fileNames)
        //{
        //    var batFilebasePath = ConfigurationManager.AppSettings["BatPath"].ToString();

        //    var batPath = Path.Combine(batFilebasePath, batFileName);

        //    var command = string.Format("{0} {1}", batPath, string.Join(" ", fileNames.ToArray()));

        //    ExecuteCommand(command);

        //}

        //private void ExecuteCommand(string command)
        //{
        //    int exitCode;
        //    ProcessStartInfo processInfo;
        //    Process process;

        //    processInfo = new ProcessStartInfo("cmd.exe", "/c " + command);
        //    processInfo.CreateNoWindow = true;
        //    processInfo.UseShellExecute = false;
        //    // *** Redirect the output ***
        //    processInfo.RedirectStandardError = true;
        //    processInfo.RedirectStandardOutput = true;

        //    processInfo.WorkingDirectory = ConfigurationManager.AppSettings["BatPath"].ToString();
        //    process = Process.Start(processInfo);
        //    process.WaitForExit();

        //    // *** Read the streams ***
        //    // Warning: This approach can lead to deadlocks, see Edit #2
        //    string output = process.StandardOutput.ReadToEnd();
        //    string error = process.StandardError.ReadToEnd();

        //    exitCode = process.ExitCode;

        //    Console.WriteLine("output>>" + (string.IsNullOrEmpty(output) ? "(none)" : output));
        //    Console.WriteLine("error>>" + (string.IsNullOrEmpty(error) ? "(none)" : error));
        //    Console.WriteLine("ExitCode: " + exitCode.ToString(), "ExecuteCommand");
        //    process.Close();

        //    if (!string.IsNullOrEmpty(error))
        //    {
        //        throw new Exception(string.Format("Error was thrown while execution bat file: [{0}]", error));
        //    }
        //}
    }
}
