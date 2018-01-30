﻿using Gcode.Utils;
using Gcode.Utils.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gcode.TestSuite
{
	/// <summary>
	/// Gcode command tests
	/// </summary>
	[TestClass]
	public class GcodeParserTests
	{
		[TestMethod]
		public void FrameSetIsCommentTest1()
		{
			var s = "; head speed 63.800003, filament speed 0.000000, preload 0.000000";
			var res = new GcodeParser(s).IsComment;
			Assert.IsTrue(res);
		}
		[TestMethod]
		public void FrameSetIsCommentTest2()
		{
			var s = "   ; head speed 63.800003, filament speed 0.000000, preload 0.000000";

			var res = new GcodeParser(s).IsComment;
			Assert.IsTrue(res);
		}
		[TestMethod]
		public void FrameSetIsCommentTest3()
		{
			var s = " head speed 63.800003, filament speed 0.000000, preload 0.000000";
			var res = new GcodeParser(s).IsComment;
			Assert.IsFalse(res);
		}
		[TestMethod]
		public void FrameSetIsCommentTest4()
		{
			var s = "; 'Destring/Wipe/Jump Path', 0.0 [feed mm/s], 63.8 [head mm/s]";
			var res = new GcodeParser(s).IsComment;
			Assert.IsTrue(res);
		}
		[TestMethod]
		public void FrameSetIsCommentTest5()
		{
			var s = ";";
			var res = new GcodeParser(s).IsComment;
			Assert.IsTrue(res);
		}
		[TestMethod]
		public void FrameSetIsCommentTest6()
		{
			var s = "     ;     ";
			var res = new GcodeParser(s).IsComment;
			Assert.IsTrue(res);
		}
		[TestMethod]
		public void FrameSetIsCommentTest7()
		{
			var s = string.Empty;
			var res = new GcodeParser(s).IsComment;
			Assert.IsFalse(res);
		}
		[TestMethod]
		public void FrameSetIsCommentTest8()
		{
			var res = new GcodeParser(null).IsComment;
			Assert.IsFalse(res);
		}
		[TestMethod]
		public void FrameSetContainsCommentTest1()
		{
			var res = new GcodeParser(null).ContainsComment;
			Assert.IsFalse(res);
		}
		[TestMethod]
		public void FrameSetContainsCommentTest2()
		{

			var res = new GcodeParser("").ContainsComment;
			Assert.IsFalse(res);
		}
		[TestMethod]
		public void FrameSetContainsCommentTest3()
		{
			var res = new GcodeParser("; >   M206 T3 P149 X700.0 ; Y max len = 600.0").ContainsComment;
			Assert.IsFalse(res);
		}
		[TestMethod]
		public void FrameSetContainsCommentTest4()
		{
			var res = new GcodeParser("G1 X551.135 Y348.935 E1.23722;mooove").ContainsComment;
			Assert.IsTrue(res);
		}
		[TestMethod]
		public void FrameSetContainsCommentTest5()
		{
			var res = new GcodeParser("G1 X551.135 Y348.935 E1.23722  ;    mooove").ContainsComment;
			Assert.IsTrue(res);
		}
		[TestMethod]
		public void FrameSetContainsCommentTest6()
		{
			var res = new GcodeParser("G1 X551.135 Y348.935 E1.23722 ; mooove; dsddffs ;;;").ContainsComment;
			Assert.IsTrue(res);
		}
		[TestMethod]
		public void FrameSetContainsCommentTest7()
		{
			var res = new GcodeParser(";").ContainsComment;
			Assert.IsFalse(res);
		}
		[TestMethod]
		public void FrameSetIsNullOrErorFrameTest1()
		{
			var res = new GcodeParser(null).ContainsComment;
			Assert.IsFalse(res);
		}
		[TestMethod]
		public void FrameSetIsNullOrErorFrameTest2()
		{
			var res = new GcodeParser("").ContainsComment;
			Assert.IsFalse(res);
		}
		[TestMethod]
		public void FrameSetIsEmptyCommentTest1()
		{
			var res = new GcodeParser(";").EmptyComment;
			Assert.IsTrue(res);
		}
		[TestMethod]
		public void FrameSetIsEmptyCommentTest2()
		{
			var res = new GcodeParser("                          ;                         ").EmptyComment;
			Assert.IsTrue(res);
		}
		[TestMethod]
		public void NormalizeRawFrameTestSynthetic1()
		{
			const string def = "A1 B2 C3 D4 E5 F6 G7";
			var comparer = def.Replace(" ", string.Empty);
			var parser = new GcodeParser(comparer);
			var res = parser.NormalizeRawFrame();

			Assert.AreEqual(def, res);
		}

		[TestMethod]
		public void NormalizeRawFrameTestSynthetic2()
		{
			const string def = "G1 X-70 Y40 F2000 ;move to bed sensr for ex7 ";
			var comparer = def.Trim();
			var parser = new GcodeParser(comparer);
			var res = parser.NormalizeRawFrame();

			Assert.AreEqual(comparer, res);
		}
		[TestMethod]
		public void NormalizeRawFrameTestSynthetic3()
		{
			const string def = "    ; move to bed sensr for ex7  G1 X-70 Y40 F2000  ;    ;;;  ";
			var parser = new GcodeParser(def);
			var res = parser.NormalizeRawFrame();

			Assert.AreEqual(def.Trim(), res);
		}

		[TestMethod]
		public void NormalizeRawFrameTestSynthetic4()
		{
			const string def = ";";
			var parser = new GcodeParser(def);
			var res = parser.NormalizeRawFrame();

			Assert.AreEqual(def, res);
		}

		[TestMethod]
		public void NormalizeRawFrameTestSynthetic5()
		{
			const string def = "   ; >   M206 T3 P200 X89 ; extruder normal steps per mm  ";
			var parser = new GcodeParser(def);
			var res = parser.NormalizeRawFrame();

			Assert.AreEqual(def.Trim(), res);
		}
		[TestMethod]
		public void NormalizeRawFrameTestReal_Real_Cube()
		{
			var data = TestSuiteDataSource.ReadTextFromFile("100.gcode").Split("\r\n");
			foreach (var d in data)
			{
				var parser = new GcodeParser(d);
				var res = parser.NormalizeRawFrame();
				Assert.IsNotNull(res);
			}
		}
		[TestMethod]
		public void NormalizeRawFrameTestReal1()
		{
			var data = TestSuiteDataSource.ReadTextFromFile("pattern_blade_fp_piece2_v1.gcode").Split("\r\n");
			foreach (var d in data)
			{
				var parser = new GcodeParser(d);
				var res = parser.NormalizeRawFrame();
				Assert.IsNotNull(res);
			}

		}
		[TestMethod]
		public void NormalizeRawFrameTestReal2()
		{
			var data = TestSuiteDataSource.ReadTextFromFile("28.gcode.modified.gcode").Split("\r\n");
			foreach (var d in data)
			{
				var parser = new GcodeParser(d);
				if (!parser.IsComment)
				{
					var res = parser.NormalizeRawFrame();
					Assert.IsNotNull(res);
				}
			}
		}
		[TestMethod]
		public void HandleSegmentsTest1()
		{
			var rawFrame = "G1 X602.368 Y348.476 E0.23255";
			var parser = new GcodeParser(rawFrame);
			var items = parser.HandleSegments();
			Assert.IsNotNull(items);
		}
		[TestMethod]
		public void HandleSegmentsTest2()
		{
			var rawFrame = "G1 X602.368 Y348.476 E0.23255";
			var parser = new GcodeParser(rawFrame);
			var items = parser.HandleSegments();
			Assert.IsTrue(items.Count > 0);
		}
		[TestMethod]
		public void HandleSegmentsTest3()
		{
			var rawFrame = "G1 X602.368 Y348.476 E0.23255";
			var rawSplited = rawFrame.Split(" ");
			var parser = new GcodeParser(rawFrame);
			var items = parser.HandleSegments();
			Assert.IsTrue(items.Count == rawSplited.Length);
		}
		[TestMethod]
		public void HandleSegmentsTestRealData1()
		{
			var data = TestSuiteDataSource.ReadTextFromFile("pattern_blade_fp_piece2_v1.gcode").Split("\r\n");
			foreach (var d in data)
			{
				var parser = new GcodeParser(d);
				if (!parser.IsComment && !parser.ContainsComment && !parser.IsNullOrErorFrame)
				{
					var res = parser.NormalizeRawFrame();
					var rawSplited = res.Split(" ");
					var items = parser.HandleSegments();
					Assert.IsTrue(items.Count == rawSplited.Length);
				}
			}
		}
	}
}
