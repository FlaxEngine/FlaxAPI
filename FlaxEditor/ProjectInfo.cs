// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using FlaxEngine;
using FlaxEngine.Json;

namespace FlaxEditor
{
    /// <summary>
    /// Project metadata loaded from the project root file.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ProjectInfo
    {
        /// <summary>
        /// The project name (used by the launcher).
        /// </summary>
        public string Name;

        /// <summary>
        /// The default scene asset identifier to open on project startup.
        /// </summary>
        public Guid DefaultSceneId;

        /// <summary>
        /// The default scene spawn point (position and view direction).
        /// </summary>
        public Ray DefaultSceneSpawn;

        /// <summary>
        /// The minimum version (engine build number) supported by this project. See <see cref="Globals.BuildNumber"/>.
        /// </summary>
        public int MinVersionSupported;

        /// <summary>
        /// Saves the project info to the file.
        /// </summary>
        public void Save()
        {
            var projectFilePath = Path.Combine(Globals.ProjectFolder, "Project.xml");

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "\t";
            settings.OmitXmlDeclaration = true;
            settings.Encoding = Encoding.Unicode;

            using (var writer = XmlWriter.Create(projectFilePath, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("Project");

                writer.WriteElementString("Name", Name);
                writer.WriteElementString("DefaultSceneId", JsonSerializer.GetStringID(DefaultSceneId));
                writer.WriteStartElement("DefaultSceneSpawnPos");
                writer.WriteElementString("X", DefaultSceneSpawn.Position.X.ToString());
                writer.WriteElementString("Y", DefaultSceneSpawn.Position.Y.ToString());
                writer.WriteElementString("Z", DefaultSceneSpawn.Position.Z.ToString());
                writer.WriteEndElement();
                writer.WriteStartElement("DefaultSceneSpawnDir");
                var directionAngle = Quaternion.LookRotation(DefaultSceneSpawn.Direction, Vector3.Up).EulerAngles;
                writer.WriteElementString("X", directionAngle.X.ToString());
                writer.WriteElementString("Y", directionAngle.Y.ToString());
                writer.WriteElementString("Z", directionAngle.Z.ToString());
                writer.WriteEndElement();
                writer.WriteElementString("MinVersionSupported", MinVersionSupported.ToString());

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }
    }
}
