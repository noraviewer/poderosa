/*
 * Copyright 2004,2006 The Poderosa Project.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 *
 * $Id: Util.cs,v 1.4 2012/03/17 14:34:23 kzmi Exp $
 */
using System;
using System.Diagnostics;
using System.Collections;
using System.Text;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

//using Microsoft.JScript;
using System.CodeDom.Compiler;

#if UNITTEST
using System.Configuration;
using NUnit.Framework;
#endif

using Poderosa.Boot;

namespace Poderosa {
    /// <summary>
    /// <ja>
    /// �W���I�Ȑ����^���s�������܂��B
    /// </ja>
    /// <en>
    /// A standard success/failure is shown. 
    /// </en>
    /// </summary>
    public enum GenericResult {
        /// <summary>
        /// <ja>�������܂���</ja>
        /// <en>Succeeded</en>
        /// </summary>
        Succeeded,
        /// <summary>
        /// <ja>���s���܂���</ja>
        /// <en>Failed</en>
        /// </summary>
        Failed
    }

    //Debug.WriteLineIf������Ŏg�p
    /// <summary>
    /// 
    /// </summary>
    /// <exclude/>
    public static class DebugOpt {
#if DEBUG
        public static bool BuildToolBar = false;
        public static bool CommandPopup = false;
        public static bool DrawingPerformance = false;
        public static bool DumpDocumentRelation = false;
        public static bool IntelliSense = false;
        public static bool IntelliSenseMenu = false;
        public static bool LogViewer = false;
        public static bool Macro = false;
        public static bool MRU = false;
        public static bool PromptRecog = false;
        public static bool Socket = false;
        public static bool SSH = false;
        public static bool ViewManagement = false;
        public static bool WebBrowser = false;
#else //RELEASE
        public static bool BuildToolBar = false;
        public static bool CommandPopup = false;
        public static bool DrawingPerformance = false;
        public static bool DumpDocumentRelation = false;
        public static bool IntelliSense = false;
        public static bool IntelliSenseMenu = false;
        public static bool LogViewer = false;
        public static bool Macro = false;
        public static bool MRU = false;
        public static bool PromptRecog = false;
        public static bool Socket = false;
        public static bool SSH = false;
        public static bool ViewManagement = false;
        public static bool WebBrowser = false;
#endif
    }


    /// <summary>
    /// 
    /// </summary>
    /// <exclude/>
    public static class RuntimeUtil {
        public static void ReportException(Exception ex) {
            Debug.WriteLine(ex.Message);
            Debug.WriteLine(ex.StackTrace);

            string errorfile = ReportExceptionToFile(ex);

            //���b�Z�[�W�{�b�N�X�Œʒm�B
            //�������̒��ŗ�O���������邱�Ƃ�SP1�ł͂���炵���B�����������Ȃ�ƃA�v���������I�����B
            //Win32�̃��b�Z�[�W�{�b�N�X���o���Ă������B�X�e�[�^�X�o�[�Ȃ���v�̂悤��
            try {
                string msg = String.Format(InternalPoderosaWorld.Strings.GetString("Message.Util.InternalError"), errorfile, ex.Message);
                MessageBox.Show(msg, "Poderosa", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            catch (Exception ex2) {
                Debug.WriteLine("(MessageBox.Show() failed) " + ex2.Message);
                Debug.WriteLine(ex2.StackTrace);
            }
        }
        public static void SilentReportException(Exception ex) {
            Debug.WriteLine(ex.Message);
            Debug.WriteLine(ex.StackTrace);
            ReportExceptionToFile(ex);
        }
        public static void DebuggerReportException(Exception ex) {
            Debug.WriteLine(ex.Message);
            Debug.WriteLine(ex.StackTrace);
        }
        //�t�@�C������Ԃ�
        private static string ReportExceptionToFile(Exception ex) {
            string errorfile = null;
            //�G���[�t�@�C���ɒǋL
            StreamWriter sw = null;
            try {
                sw = GetErrorLog(ref errorfile);
                ReportExceptionToStream(ex, sw);
            }
            finally {
                if (sw != null)
                    sw.Close();
            }
            return errorfile;
        }
        private static void ReportExceptionToStream(Exception ex, StreamWriter sw) {
            sw.WriteLine(DateTime.Now.ToString());
            sw.WriteLine(ex.Message);
            sw.WriteLine(ex.StackTrace);
            //inner exception������
            Exception i = ex.InnerException;
            while (i != null) {
                sw.WriteLine("[inner] " + i.Message);
                sw.WriteLine(i.StackTrace);
                i = i.InnerException;
            }
        }
        private static StreamWriter GetErrorLog(ref string errorfile) {
            errorfile = PoderosaStartupContext.Instance.ProfileHomeDirectory + "error.log";
            return new StreamWriter(errorfile, true/*append!*/, Encoding.Default);
        }

        public static Font CreateFont(string name, float size) {
            try {
                return new Font(name, size);
            }
            catch (ArithmeticException) {
                //JSPager�̌��őΉ��Bmsvcr71�����[�h�ł��Ȃ��������邩������Ȃ��̂ŗ�O��������Ă͂��߂ČĂԂ悤�ɂ���
                Win32.ClearFPUOverflowFlag();
                return new Font(name, size);
            }
        }

        public static string ConcatStrArray(string[] values, char delimiter) {
            StringBuilder bld = new StringBuilder();
            for (int i = 0; i < values.Length; i++) {
                if (i > 0)
                    bld.Append(delimiter);
                bld.Append(values[i]);
            }
            return bld.ToString();
        }

        //min������min, max�ȏ��max�A����ȊO��value��Ԃ�
        public static int AdjustIntRange(int value, int min, int max) {
            Debug.Assert(min <= max);
            if (value < min)
                return min;
            else if (value > max)
                return max;
            else
                return value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <exclude/>
    public static class ParseUtil {
        public static bool ParseBool(string value, bool defaultvalue) {
            try {
                if (value == null || value.Length == 0)
                    return defaultvalue;
                return Boolean.Parse(value);
            }
            catch (Exception) {
                return defaultvalue;
            }
        }
        public static byte ParseByte(string value, byte defaultvalue) {
            try {
                if (value == null || value.Length == 0)
                    return defaultvalue;
                return Byte.Parse(value);
            }
            catch (Exception) {
                return defaultvalue;
            }
        }
        public static int ParseInt(string value, int defaultvalue) {
            try {
                if (value == null || value.Length == 0)
                    return defaultvalue;
                return Int32.Parse(value);
            }
            catch (Exception) {
                return defaultvalue;
            }
        }
        public static float ParseFloat(string value, float defaultvalue) {
            try {
                if (value == null || value.Length == 0)
                    return defaultvalue;
                return Single.Parse(value);
            }
            catch (Exception) {
                return defaultvalue;
            }
        }
        public static int ParseHexInt(string value, int defaultvalue) {
            try {
                if (value == null || value.Length == 0)
                    return defaultvalue;
                return Int32.Parse(value, System.Globalization.NumberStyles.HexNumber);
            }
            catch (Exception) {
                return defaultvalue;
            }
        }
        public static Color ParseColor(string t, Color defaultvalue) {
            if (t == null || t.Length == 0)
                return defaultvalue;
            else {
                if (t.Length == 8) { //16�i�ŕۑ�����Ă��邱�Ƃ�����B���]�̍�ł��̂悤��
                    int v;
                    if (Int32.TryParse(t, System.Globalization.NumberStyles.HexNumber, null, out v))
                        return Color.FromArgb(v);
                }
                else if (t.Length == 6) {
                    int v;
                    if (Int32.TryParse(t, System.Globalization.NumberStyles.HexNumber, null, out v))
                        return Color.FromArgb((int)((uint)v | 0xFF000000)); //'A'�v�f��0xFF��
                }
                Color c = Color.FromName(t);
                return c.ToArgb() == 0 ? defaultvalue : c; //�ւ�Ȗ��O�������Ƃ��AARGB�͑S��0�ɂȂ邪�AIsEmpty��false�B�Ȃ̂ł���Ŕ��肷�邵���Ȃ�
            }
        }

        public static T ParseEnum<T>(string value, T defaultvalue) {
            try {
                if (value == null || value.Length == 0)
                    return defaultvalue;
                else
                    return (T)Enum.Parse(typeof(T), value, false);
            }
            catch (Exception) {
                return defaultvalue;
            }
        }

        public static bool TryParseEnum<T>(string value, ref T parsed) {
            if (value == null || value.Length == 0) {
                return false;
            }

            try {
                parsed = (T)Enum.Parse(typeof(T), value, false);
                return true;
            }
            catch (Exception) {
                return false;
            }
        }

        //TODO Generics��
        public static ValueType ParseMultipleEnum(Type enumtype, string t, ValueType defaultvalue) {
            try {
                int r = 0;
                foreach (string a in t.Split(','))
                    r |= (int)Enum.Parse(enumtype, a, false);
                return r;
            }
            catch (FormatException) {
                return defaultvalue;
            }
        }
    }


#if UNITTEST
    public static class UnitTestUtil {
        public static void Trace(string text) {
            Console.Out.WriteLine(text);
            Debug.WriteLine(text);
        }

        public static void Trace(string fmt, params object[] args) {
            Trace(String.Format(fmt, args));
        }

        //configuration file���烍�[�h ���݂�Poderosa.Monolithic.config�t�@�C��������
        public static string GetUnitTestConfig(string entry_name) {
            string r = ConfigurationManager.AppSettings[entry_name];
            if (r == null)
                Assert.Fail("the entry \"{0}\" is not found in Poderosa.Monolithic.config file.", entry_name);
            return r;
        }

        public static string DumpStructuredText(StructuredText st) {
            StringWriter wr = new StringWriter();
            new TextStructuredTextWriter(wr).Write(st);
            wr.Close();
            return wr.ToString();
        }
    }

    [TestFixture]
    public class RuntimeUtilTests {
        [Test] //�����ӂ��̃P�[�X
        public void ParseColor1() {
            Color c1 = Color.Red;
            Color c2 = ParseUtil.ParseColor("Red", Color.White);
            Assert.AreEqual(c1, c2);
        }
        [Test] //hex 8�P�^��ARGB
        public void ParseColor2() {
            Color c1 = Color.FromArgb(10, 20, 30);
            Color c2 = ParseUtil.ParseColor("FF0A141E", Color.White);
            Assert.AreEqual(c1, c2);
        }
        [Test] //hex 6�P�^��RGB
        public void ParseColor3() {
            Color c1 = Color.FromArgb(10, 20, 30);
            Color c2 = ParseUtil.ParseColor("0A141E", Color.White);
            Assert.AreEqual(c1, c2);
        }
        [Test] //KnownColor�ł�OK
        public void ParseColor4() {
            Color c1 = Color.FromKnownColor(KnownColor.WindowText);
            Color c2 = ParseUtil.ParseColor("WindowText", Color.White);
            Assert.AreEqual(c1, c2);
        }
        [Test] //ARGB�͈�v�ł�Color�̔�r�Ƃ��Ă͕s��v
        public void ParseColor5() {
            Color c1 = Color.Blue;
            Color c2 = ParseUtil.ParseColor("0000FF", Color.White);
            Assert.AreNotEqual(c1, c2);
            Assert.AreEqual(c1.ToArgb(), c2.ToArgb());
        }
        [Test]�@//�m��Ȃ����O�̓��X�g�̈����ƈ�v
        public void ParseColor6() {
            Color c1 = Color.White;
            Color c2 = ParseUtil.ParseColor("asdfghj", Color.White); //�p�[�X�ł��Ȃ��ꍇ
            Assert.AreEqual(c1, c2);
        }
        [Test] //���łȂ̂Ŏd�l�m�F ToString()�ł͂��߂ł���
        public void ColorToString() {
            Color c1 = Color.Red;
            Color c2 = Color.FromName("Red");
            Color c3 = Color.FromKnownColor(KnownColor.WindowFrame);
            Color c4 = Color.FromArgb(255, 0, 0);

            Assert.AreEqual(c1, c2);
            Assert.AreEqual("Red", c1.Name);
            Assert.AreEqual("Red", c2.Name);
            Assert.AreEqual("WindowFrame", c3.Name);
            Assert.AreEqual("ffff0000", c4.Name);
        }
    }


#endif
}
