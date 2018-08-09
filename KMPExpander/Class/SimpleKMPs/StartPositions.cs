﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using LibCTR.Collections;
using KMPSections;
using System.Windows.Forms;
using System.Drawing;
using Tao.OpenGl;

namespace KMPExpander.Class.SimpleKMPs
{
    public class StartPositions : SectionBase<StartPositions.StartEntry>
    {
        //[XmlElement("Entry")]
        //public List<StartEntry> Entries = new List<StartEntry>();
        public StartPositions() { }

        public StartPositions(KTPT data)
        {
            foreach (KTPT.KTPTEntry entry in data.Entries)
                Entries.Add(new StartEntry(entry));
        }
        
        public class StartEntry
        {
            //public Vector3 Position { get; set; }
            [XmlAttribute]
            public Single PositionX { get; set; }
            [XmlAttribute]
            public Single PositionY { get; set; }
            [XmlAttribute]
            public Single PositionZ { get; set; }
            //public Vector3 Rotation { get; set; }
            [XmlAttribute]
            public Single RotationX { get; set; }
            [XmlAttribute]
            public Single RotationY { get; set; }
            [XmlAttribute]
            public Single RotationZ { get; set; }

            public StartEntry(KTPT.KTPTEntry entry)
            {
                //Position = entry.Position;
                PositionX = entry.Position.X;
                PositionY = entry.Position.Y;
                PositionZ = entry.Position.Z;
                RotationX = entry.Rotation.X;
                RotationY = entry.Rotation.Y;
                RotationZ = entry.Rotation.Z;
                //Rotation = entry.Rotation;
            }

            public StartEntry() { }

            public KTPT.KTPTEntry ToKTPTEntry(uint i)
            {
                KTPT.KTPTEntry entry = new KTPT.KTPTEntry();
                entry.Index = i;
                entry.Position = new Vector3(PositionX, PositionY, PositionZ);
                entry.Rotation = new Vector3(RotationX, RotationY, RotationZ);
                return entry;
            }

            public void RenderPicking(int entry_id)
            {
                VisualSettings Settings = (Application.OpenForms[0] as Form1).Settings;
                Color pickingColor = SectionPicking.GetColor(Sections.StartPositions, 0, entry_id);

                Gl.glPointSize(Settings.PointSize + 2f);
                Gl.glBegin(Gl.GL_POINTS);
                Gl.glColor4f(pickingColor.R / 255f, pickingColor.G / 255f, pickingColor.B / 255f, 1f);
                Gl.glVertex2f(PositionX, PositionZ);
                Gl.glEnd();
            }

            public void RenderPoint(bool picking)
            {
                VisualSettings Settings = (Application.OpenForms[0] as Form1).Settings;
                List<object> SelectedDots = (Application.OpenForms[0] as Form1).SelectedDots;

                Gl.glPointSize(Settings.PointSize + 2f);
                Gl.glBegin(Gl.GL_POINTS);
                if (SelectedDots.Contains(this)) Gl.glColor4f(Settings.HighlightPointborderColor.R / 255f, Settings.HighlightPointborderColor.G / 255f, Settings.HighlightPointborderColor.B / 255f, Settings.HighlightPointborderColor.A);
                else Gl.glColor4f(Settings.PointborderColor.R / 255f, Settings.PointborderColor.G / 255f, Settings.PointborderColor.B / 255f, Settings.PointborderColor.A);
                Gl.glVertex2f(PositionX, PositionZ);
                Gl.glEnd();

                Gl.glPointSize(Settings.PointSize);
                Gl.glBegin(Gl.GL_POINTS);
                if (SelectedDots.Contains(this)) Gl.glColor4f(Settings.HighlightPointColor.R / 255f, Settings.HighlightPointColor.G / 255f, Settings.HighlightPointColor.B / 255f, Settings.HighlightPointColor.A);
                else Gl.glColor4f(Settings.KtptColor.R / 255f, Settings.KtptColor.G / 255f, Settings.KtptColor.B / 255f, Settings.KtptColor.A);
                Gl.glVertex2f(PositionX, PositionZ);
                Gl.glEnd();
            }
        }


        public KTPT ToKTPT()
        {
            KTPT data = new KTPT();
            data.NrEntries = (uint)Entries.Count;
            uint i = 0;
            foreach (var entry in Entries)
            {
                data.Entries.Add(entry.ToKTPTEntry(i));
                i++;
            }
            return data;
        }

        public TreeNode GetTreeNode()
        {
            TreeNode node = new TreeNode("Start Positions");
            node.Tag = this;
            return node;
        }

        public void Render(bool picking)
        {
            if (!Visible) return;

            if (picking)
            {
                for (int i = 0; i < Entries.Count; i++)
                    Entries[i].RenderPicking(i);
                return;
            }

            foreach (var entry in Entries)
                entry.RenderPoint(picking);
        }
    }
}
