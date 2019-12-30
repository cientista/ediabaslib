﻿using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Reflection;
using System.Text;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;

namespace ExtractEcuFunctions
{
    public class EcuVariant
    {
        public EcuVariant(string id, string groupId, List<string> groupFunctionIds)
        {
            Id = id;
            GroupId = groupId;
            GroupFunctionIds = groupFunctionIds;
        }

        public string ToString(string prefix)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(prefix + "VARIANT:");
            sb.Append(this.PropertyList(prefix + " "));
            if (GroupFunctionIds != null)
            {
                foreach (string GroupFunctionId in GroupFunctionIds)
                {
                    sb.Append(prefix + " " + GroupFunctionId);
                }
            }
            return sb.ToString();
        }

        public override string ToString()
        {
            return ToString("");
        }

        public string Id { get; }
        public string GroupId { get; }
        public List<string> GroupFunctionIds { get; }
    }

    public class EcuVarFunc
    {
        public EcuVarFunc(string id, string groupFuncId, EcuVariant ecuVariant)
        {
            Id = id;
            GroupFuncId = groupFuncId;
            EcuVariant = ecuVariant;
        }

        public string ToString(string prefix)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(prefix + "VARFUNC:");
            sb.Append(this.PropertyList(prefix + " "));
            return sb.ToString();
        }

        public override string ToString()
        {
            return ToString("");
        }

        public string Id { get; }
        public string GroupFuncId { get; }
        public EcuVariant EcuVariant { get; }
    }

    public class EcuFuncStruct
    {
        public EcuFuncStruct(string id, string titleEn, string titleDe, string titleRu, EcuVarFunc ecuVarFunc)
        {
            Id = id;
            TitleEn = titleEn;
            TitleDe = titleDe;
            TitleRu = titleRu;
            EcuVarFunc = ecuVarFunc;
        }

        public string ToString(string prefix)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(prefix + "FUNC:");
            sb.Append(this.PropertyList(prefix + " "));
            if (FixedFuncStructList != null)
            {
                foreach (EcuFixedFuncStruct ecuFixedFuncStruct in FixedFuncStructList)
                {
                    sb.Append(ecuFixedFuncStruct.ToString(prefix + " "));
                }
            }

            return sb.ToString();
        }

        public override string ToString()
        {
            return ToString("");
        }

        public string Id { get; }
        public string TitleEn { get; }
        public string TitleDe { get; }
        public string TitleRu { get; }
        public EcuVarFunc EcuVarFunc { get; }
        public List<EcuFixedFuncStruct> FixedFuncStructList { get; set; }
    }

    public class EcuJob
    {
        public EcuJob(string id, string funcNameJob, string name, EcuFixedFuncStruct ecuFixedFuncStruct)
        {
            Id = id;
            FuncNameJob = funcNameJob;
            Name = name;
            EcuFixedFuncStruct = ecuFixedFuncStruct;
        }

        public string ToString(string prefix)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(prefix + "JOB:");
            sb.Append(this.PropertyList(prefix + " "));
            if (EcuJobParList != null)
            {
                foreach (EcuJobParameter ecuJobParameter in EcuJobParList)
                {
                    sb.Append(ecuJobParameter.ToString(prefix + " "));
                }
            }

            if (EcuJobResultList != null)
            {
                foreach (EcuJobResult ecuJobResult in EcuJobResultList)
                {
                    sb.Append(ecuJobResult.ToString(prefix + " "));
                }
            }

            return sb.ToString();
        }

        public override string ToString()
        {
            return ToString("");
        }

        public string Id { get; }
        public string FuncNameJob { get; }
        public string Name { get; }
        public EcuFixedFuncStruct EcuFixedFuncStruct { get; }
        public List<EcuJobParameter> EcuJobParList { get; set; }
        public List<EcuJobResult> EcuJobResultList { get; set; }
    }

    public class EcuJobParameter
    {
        public EcuJobParameter(string id, string value, string adapterPath, string name, EcuJob ecuJob)
        {
            Id = id;
            Value = value;
            AdapterPath = adapterPath;
            Name = name;
            EcuJob = ecuJob;
        }

        public string ToString(string prefix)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(prefix + "JOB:");
            sb.Append(this.PropertyList(prefix + " "));
            return sb.ToString();
        }

        public override string ToString()
        {
            return ToString("");
        }

        public string Id { get; }
        public string Value { get; }
        public string AdapterPath { get; }
        public string Name { get; }
        public EcuJob EcuJob { get; }
    }

    public class EcuJobResult
    {
        public EcuJobResult(string id, string titleEn, string titleDe, string titleRu, string adapterPath,
            string name, string unit, string unitFixed, string format, string mult, string offset, string round, EcuJob ecuJob)
        {
            Id = id;
            TitleEn = titleEn;
            TitleDe = titleDe;
            TitleRu = titleRu;
            AdapterPath = adapterPath;
            Name = name;
            Unit = unit;
            UnitFixed = unitFixed;
            Format = format;
            Mult = mult;
            Offset = offset;
            Round = round;
            EcuJob = ecuJob;
        }

        public string ToString(string prefix)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(prefix + "RESULT:");
            sb.Append(this.PropertyList(prefix + " "));
            return sb.ToString();
        }

        public override string ToString()
        {
            return ToString("");
        }

        public string Id { get; }
        public string TitleEn { get; }
        public string TitleDe { get; }
        public string TitleRu { get; }
        public string AdapterPath { get; }
        public string Name { get; }
        public string Unit { get; }
        public string UnitFixed { get; }
        public string Format { get; }
        public string Mult { get; }
        public string Offset { get; }
        public string Round { get; }
        public EcuJob EcuJob { get; }
    }

    public class EcuFixedFuncStruct
    {
        public EcuFixedFuncStruct(string id, string nodeClass, string nodeClassName, string titleEn, string titleDe, string titleRu,
            string prepOpEn, string prepOpDe, string prepOpRu,
            string procOpEn, string procOpDe, string procOpRu,
            string postOpEn, string postOpDe, string postOpRu,
            EcuFuncStruct ecuFuncStruct)
        {
            Id = id;
            NodeClass = nodeClass;
            NodeClassName = nodeClassName;
            TitleEn = titleEn;
            TitleDe = titleDe;
            TitleRu = titleRu;
            PrepOpEn = prepOpEn;
            PrepOpDe = prepOpDe;
            PrepOpRu = prepOpRu;
            ProcOpEn = procOpEn;
            ProcOpDe = procOpDe;
            ProcOpRu = procOpRu;
            PostOpEn = postOpEn;
            PostOpDe = postOpDe;
            PostOpRu = postOpRu;
            EcuFuncStruct = ecuFuncStruct;
        }

        public string ToString(string prefix)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(prefix + "FIXEDFUNC:");
            sb.Append(this.PropertyList(prefix + " "));
            if (EcuJobList != null)
            {
                foreach (EcuJob ecuJob in EcuJobList)
                {
                    sb.Append(ecuJob.ToString(prefix + " "));
                }
            }

            return sb.ToString();
        }

        public override string ToString()
        {
            return ToString("");
        }

        public string Id { get; }
        public string NodeClass { get; }
        public string NodeClassName { get; }
        public string TitleEn { get; }
        public string TitleDe { get; }
        public string TitleRu { get; }
        public string PrepOpEn { get; }
        public string PrepOpDe { get; }
        public string PrepOpRu { get; }
        public string ProcOpEn { get; }
        public string ProcOpDe { get; }
        public string ProcOpRu { get; }
        public string PostOpEn { get; }
        public string PostOpDe { get; }
        public string PostOpRu { get; }
        public EcuFuncStruct EcuFuncStruct { get; }
        public List<EcuJob> EcuJobList { get; set; }
    }

    static class Program
    {
        static int Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;
            TextWriter outTextWriter = args.Length > 0 ? Console.Out : null;

            if (args.Length < 1)
            {
                outTextWriter?.WriteLine("No Database specified");
                return 1;
            }
            if (args.Length < 2)
            {
                outTextWriter?.WriteLine("No output directory specified");
                return 1;
            }
            if (args.Length < 3)
            {
                outTextWriter?.WriteLine("No ECU name specified");
                return 1;
            }

            string outDir = args[1];
            if (string.IsNullOrEmpty(outDir))
            {
                outTextWriter?.WriteLine("Output directory empty");
                return 1;
            }

            string ecuName = args[2];
            if (string.IsNullOrEmpty(ecuName))
            {
                outTextWriter?.WriteLine("ECU name empty");
                return 1;
            }

            try
            {
                string connection = "Data Source=\"" + args[0] + "\";";
                using (SQLiteConnection mDbConnection = new SQLiteConnection(connection))
                {
                    mDbConnection.SetPassword("6505EFBDC3E5F324");
                    mDbConnection.Open();

                    List<EcuFuncStruct> ecuFuncStructList = GetEcuFuncStructList(outTextWriter, mDbConnection, ecuName);

                    mDbConnection.Close();

                    if (ecuFuncStructList == null)
                    {
                        return 1;
                    }

                    foreach (EcuFuncStruct ecuFuncStruct in ecuFuncStructList)
                    {
                        outTextWriter?.WriteLine(ecuFuncStruct);
                    }
                }
            }
            catch (Exception e)
            {
                outTextWriter?.WriteLine(e);
            }
            return 0;
        }

        public static string PropertyList(this object obj, string prefix)
        {
            StringBuilder sb = new StringBuilder();
            PropertyInfo[] props = obj.GetType().GetProperties();
            foreach (PropertyInfo p in props)
            {
                if (p.PropertyType == typeof(string))
                {
                    string value = p.GetValue(obj, null).ToString();
                    if (!string.IsNullOrEmpty(value))
                    {
                        sb.AppendLine(prefix + p.Name + ": " + value);
                    }
                }
            }
            return sb.ToString();
        }

        public static string PropertyList(this object obj)
        {
            return obj.PropertyList("");
        }

        private static EcuVariant GetEcuVariant(SQLiteConnection mDbConnection, string ecuName)
        {
            EcuVariant ecuVariant = null;
            string sql = string.Format(@"SELECT ID, ECUGROUPID FROM XEP_ECUVARIANTS WHERE (lower(NAME) = '{0}')", ecuName.ToLowerInvariant());
            SQLiteCommand command = new SQLiteCommand(sql, mDbConnection);
            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    string groupId = reader["ECUGROUPID"].ToString();
                    ecuVariant = new EcuVariant(reader["ID"].ToString(),
                        groupId,
                        GetEcuGroupFunctionIds(mDbConnection, groupId));
                }
            }

            return ecuVariant;
        }

        private static List<string> GetEcuGroupFunctionIds(SQLiteConnection mDbConnection, string groupId)
        {
            List<string> ecuGroupFunctionIds = new List<string>();
            string sql = string.Format(@"SELECT ID FROM XEP_ECUGROUPFUNCTIONS WHERE ECUGROUPID = {0}", groupId);
            SQLiteCommand command = new SQLiteCommand(sql, mDbConnection);
            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    ecuGroupFunctionIds.Add(reader["ID"].ToString());
                }
            }

            return ecuGroupFunctionIds;
        }

        private static string GetNodeClassName(SQLiteConnection mDbConnection, string nodeClass)
        {
            string sql = string.Format(@"SELECT NAME FROM XEP_NODECLASSES WHERE ID = {0}", nodeClass);
            SQLiteCommand command = new SQLiteCommand(sql, mDbConnection);
            string result = string.Empty;
            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result = reader["NAME"].ToString();
                }
            }

            return result;
        }

        private static List<EcuFuncStruct> GetEcuFuncStructList(TextWriter outTextWriter, SQLiteConnection mDbConnection, string ecuName)
        {
            EcuVariant ecuVariant = GetEcuVariant(mDbConnection, ecuName);
            if (ecuVariant == null)
            {
                outTextWriter?.WriteLine("ECU variant not found");
                return null;
            }

            outTextWriter?.WriteLine(ecuVariant);

            List<EcuVarFunc> ecuVarFunctionsList = new List<EcuVarFunc>();
            foreach (string ecuGroupFunctionId in ecuVariant.GroupFunctionIds)
            {
                string sql = string.Format(@"SELECT ID, VISIBLE, NAME, OBD_RELEVANZ FROM XEP_ECUVARFUNCTIONS WHERE (lower(NAME) = '{0}') AND (ECUGROUPFUNCTIONID = {1})", ecuName.ToLowerInvariant(), ecuGroupFunctionId);
                SQLiteCommand command = new SQLiteCommand(sql, mDbConnection);
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ecuVarFunctionsList.Add(new EcuVarFunc(reader["ID"].ToString(), ecuGroupFunctionId, ecuVariant));
                    }
                }
            }

            if (ecuVarFunctionsList.Count == 0)
            {
                outTextWriter?.WriteLine("ECU var functions not found");
                return null;
            }

            foreach (EcuVarFunc ecuVarFunc in ecuVarFunctionsList)
            {
                outTextWriter?.WriteLine(ecuVarFunc);
            }

            List<EcuFuncStruct> ecuFuncStructList = new List<EcuFuncStruct>();
            foreach (EcuVarFunc ecuVarFunc in ecuVarFunctionsList)
            {
                string sql = string.Format(@"SELECT REFFUNCS.ECUFUNCSTRUCTID FUNCSTRUCTID, TITLE_ENUS, TITLE_DEDE, TITLE_RU " +
                        "FROM XEP_ECUFUNCSTRUCTURES FUNCS, XEP_REFECUFUNCSTRUCTS REFFUNCS WHERE FUNCS.ID = REFFUNCS.ECUFUNCSTRUCTID AND REFFUNCS.ID = {0}", ecuVarFunc.Id);
                SQLiteCommand command = new SQLiteCommand(sql, mDbConnection);
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ecuFuncStructList.Add(new EcuFuncStruct(reader["FUNCSTRUCTID"].ToString(),
                            reader["TITLE_ENUS"].ToString(),
                            reader["TITLE_DEDE"].ToString(),
                            reader["TITLE_RU"].ToString(),
                            ecuVarFunc));
                    }
                }
            }

            if (ecuFuncStructList.Count == 0)
            {
                outTextWriter?.WriteLine("ECU function structures not found");
                return null;
            }

            foreach (EcuFuncStruct ecuFuncStruct in ecuFuncStructList)
            {
                List<EcuFixedFuncStruct> ecuFixedFuncStructList = new List<EcuFixedFuncStruct>();
                string sql = string.Format(@"SELECT ID, NODECLASS, TITLE_ENUS, TITLE_DEDE, TITLE_RU, " +
                                           "PREPARINGOPERATORTEXT_ENUS, PREPARINGOPERATORTEXT_DEDE, PREPARINGOPERATORTEXT_RU, " +
                                           "PROCESSINGOPERATORTEXT_ENUS, PROCESSINGOPERATORTEXT_DEDE, PROCESSINGOPERATORTEXT_RU, " +
                                           "POSTOPERATORTEXT_ENUS, POSTOPERATORTEXT_DEDE, POSTOPERATORTEXT_RU " +
                                           "FROM XEP_ECUFIXEDFUNCTIONS WHERE (PARENTID = {0})", ecuFuncStruct.Id);
                SQLiteCommand command = new SQLiteCommand(sql, mDbConnection);
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string nodeClass = reader["NODECLASS"].ToString();
                        ecuFixedFuncStructList.Add(new EcuFixedFuncStruct(reader["ID"].ToString(),
                            nodeClass,
                            GetNodeClassName(mDbConnection, nodeClass),
                            reader["TITLE_ENUS"].ToString(),
                            reader["TITLE_DEDE"].ToString(),
                            reader["TITLE_RU"].ToString(),
                            reader["PREPARINGOPERATORTEXT_ENUS"].ToString(),
                            reader["PREPARINGOPERATORTEXT_DEDE"].ToString(),
                            reader["PREPARINGOPERATORTEXT_RU"].ToString(),
                            reader["PROCESSINGOPERATORTEXT_ENUS"].ToString(),
                            reader["PROCESSINGOPERATORTEXT_DEDE"].ToString(),
                            reader["PROCESSINGOPERATORTEXT_RU"].ToString(),
                            reader["POSTOPERATORTEXT_ENUS"].ToString(),
                            reader["POSTOPERATORTEXT_DEDE"].ToString(),
                            reader["POSTOPERATORTEXT_RU"].ToString(),
                            ecuFuncStruct));
                    }
                }

                if (ecuFixedFuncStructList.Count == 0)
                {
                    outTextWriter?.WriteLine("ECU fixed function structures not found");
                    return null;
                }

                ecuFuncStruct.FixedFuncStructList = ecuFixedFuncStructList;
            }

            foreach (EcuFuncStruct ecuFuncStruct in ecuFuncStructList)
            {
                foreach (EcuFixedFuncStruct ecuFixedFuncStruct in ecuFuncStruct.FixedFuncStructList)
                {
                    List<EcuJob> ecuJobList = new List<EcuJob>();
                    string sql = string.Format(@"SELECT JOBS.ID JOBID, FUNCTIONNAMEJOB, NAME " +
                                               "FROM XEP_ECUJOBS JOBS, XEP_REFECUJOBS REFJOBS WHERE JOBS.ID = REFJOBS.ECUJOBID AND REFJOBS.ID = {0}",
                        ecuFixedFuncStruct.Id);
                    SQLiteCommand command = new SQLiteCommand(sql, mDbConnection);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ecuJobList.Add(new EcuJob(reader["JOBID"].ToString(),
                                reader["FUNCTIONNAMEJOB"].ToString(),
                                reader["NAME"].ToString(),
                                ecuFixedFuncStruct));
                        }
                    }

                    if (ecuJobList.Count == 0)
                    {
                        outTextWriter?.WriteLine("ECU jobs not found");
                        return null;
                    }

                    foreach (EcuJob ecuJob in ecuJobList)
                    {
                        List<EcuJobParameter> ecuJobParList = new List<EcuJobParameter>();
                        sql = string.Format(
                            @"SELECT PARAM.ID PARAMID, PARAMVALUE, FUNCTIONNAMEPARAMETER, ADAPTERPATH, NAME, ECUJOBID " +
                            "FROM XEP_ECUPARAMETERS PARAM, XEP_REFECUPARAMETERS REFPARAM WHERE " +
                            "PARAM.ID = REFPARAM.ECUPARAMETERID AND REFPARAM.ID = {0} AND PARAM.ECUJOBID = {1}",
                            ecuFixedFuncStruct.Id, ecuJob.Id);
                        command = new SQLiteCommand(sql, mDbConnection);
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ecuJobParList.Add(new EcuJobParameter(reader["PARAMID"].ToString(),
                                    reader["PARAMVALUE"].ToString(),
                                    reader["ADAPTERPATH"].ToString(),
                                    reader["NAME"].ToString(),
                                    ecuJob));
                            }
                        }

                        ecuJob.EcuJobParList = ecuJobParList;

                        List<EcuJobResult> ecuJobResultList = new List<EcuJobResult>();
                        sql = string.Format(
                            @"SELECT RESULTS.ID RESULTID, TITLE_ENUS, TITLE_DEDE, TITLE_RU, ADAPTERPATH, NAME, UNIT, UNITFIXED, FORMAT, MULTIPLIKATOR, OFFSET, RUNDEN, ECUJOBID " +
                            "FROM XEP_ECURESULTS RESULTS, XEP_REFECURESULTS REFRESULTS WHERE " +
                            "ECURESULTID = RESULTS.ID AND REFRESULTS.ID = {0} AND RESULTS.ECUJOBID = {1}",
                            ecuFixedFuncStruct.Id, ecuJob.Id);
                        command = new SQLiteCommand(sql, mDbConnection);
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ecuJobResultList.Add(new EcuJobResult(reader["RESULTID"].ToString(),
                                    reader["TITLE_ENUS"].ToString(),
                                    reader["TITLE_DEDE"].ToString(),
                                    reader["TITLE_RU"].ToString(),
                                    reader["ADAPTERPATH"].ToString(),
                                    reader["NAME"].ToString(),
                                    reader["UNIT"].ToString(),
                                    reader["UNITFIXED"].ToString(),
                                    reader["FORMAT"].ToString(),
                                    reader["MULTIPLIKATOR"].ToString(),
                                    reader["OFFSET"].ToString(),
                                    reader["RUNDEN"].ToString(),
                                    ecuJob));
                            }
                        }

                        ecuJob.EcuJobResultList = ecuJobResultList;
                    }

                    ecuFixedFuncStruct.EcuJobList = ecuJobList;
                }
            }

            return ecuFuncStructList;
        }

        // ReSharper disable once UnusedMember.Local
        private static bool CreateZip(List<string> inputFiles, string archiveFilenameOut)
        {
            try
            {
                if (File.Exists(archiveFilenameOut))
                {
                    File.Delete(archiveFilenameOut);
                }
                FileStream fsOut = File.Create(archiveFilenameOut);
                ZipOutputStream zipStream = new ZipOutputStream(fsOut);
                zipStream.SetLevel(3);

                try
                {
                    foreach (string filename in inputFiles)
                    {

                        FileInfo fi = new FileInfo(filename);
                        string entryName = Path.GetFileName(filename);

                        ZipEntry newEntry = new ZipEntry(entryName)
                        {
                            DateTime = fi.LastWriteTime,
                            Size = fi.Length
                        };
                        zipStream.PutNextEntry(newEntry);

                        byte[] buffer = new byte[4096];
                        using (FileStream streamReader = File.OpenRead(filename))
                        {
                            StreamUtils.Copy(streamReader, zipStream, buffer);
                        }
                        zipStream.CloseEntry();
                    }
                }
                finally
                {
                    zipStream.IsStreamOwner = true;
                    zipStream.Close();
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
