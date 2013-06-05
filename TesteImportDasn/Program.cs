using System;
using System.IO;
using System.Linq;
using FlatFileImport;
using FlatFileImport.Core;
using FlatFileImport.Input;
using FlatFileImport.Process;
using System.Collections.Generic;

namespace TesteImportDasn
{
	class CompilerPolicyTest : ICompilerPolicy
	{
		private IList<IRawLine> _lines;
		private string _codTom;

		public CompilerPolicyTest(string codTom)
		{
			_codTom = codTom;
		}

		#region IComplierPolicy Members

		public bool IsValid
		{
			get
			{
				var a = _lines.FirstOrDefault(s => s.Value.StartsWith("00000"));
				var tom = String.Format("|{0}|", _codTom);
				if (a == null)
					return false;

				if (a.Value.Contains(tom))
					return true;

				var b = _lines.Where(s => s.Value.StartsWith("03000"));

				return b.Any(d => d.Value.Contains(tom));
			}
		}

		public string HeaderIdentifier
		{
			get { return "P0000"; }
		}

		public string FooterIdentifier
		{
			get { return "P9999"; }
		}

		public void OnChunkRead(IList<IRawLine> rawLines)
		{
			_lines = rawLines;
		}

		#endregion
	}

	class Program
	{
		private static string _path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Samples\Files");

		private static string GetVersion(IFileInfo toParse)
		{
			if (String.IsNullOrEmpty(toParse.Header))
				throw new NullReferenceException("A definição do Header, do FileInfo está fazia." + toParse.Path);

			string[] values = toParse.Header.Split("|".ToCharArray());

			if (values == null || values.Length <= 0)
				throw new NullReferenceException("Não foi possivel fazer o parse para verificar a versão do arquivo" + toParse.Path);

			if (values[0] != "AAAAA")
				throw new Exception("O campo AAAAA não existe no arquivo");

			return values[1];
		}

		private static void Main(string[] args)
		{
			Run_ExemploComCompilerPolicy();
			Run_Default();
		}

		private static void Run_ExemploComCompilerPolicy()
		{
			IBlueprintFactoy factoy = new BlueprintFactory();
			var importer = new Importer();
			importer.CompilerPolicy = new CompilerPolicyTest("5401");
			var handler = HandlerFacotry.Handler(Path.Combine(_path, "02-3105-DAS-20090816-01-INNER.txt"));
			var view = new ViewMain();

			importer.NotifyLine = true;
			importer.RegisterObserver(view);


			view.FilePath = handler.Path;

			var bPrint = factoy.GetBlueprint("PGDAS", handler.FileInfo);

			if (bPrint == null)
			{
				var path = AppDomain.CurrentDomain.BaseDirectory;

				using (var file = new StreamWriter(path + "Versoes.txt", true))
				{
					file.WriteLine("\nVersão: {0} | File: {1}\n", GetVersion(handler.FileInfo), handler.FileInfo.Path);
				}
			}


			importer.SetBlueprint(bPrint);
			importer.SetFileToProcess(handler.FileInfo);
			importer.Valid();

			if (importer.IsValid)
				importer.Process();

			importer.Reset();
		}

		private static void Run_Default()
		{
			var view = new ViewMain();

			IBlueprintFactoy factoy = new BlueprintFactory();
			var importer = new Importer();
			IHandlerCollection handler = null;

			try
			{
				//var handler = Handler.GetHandler(Path.Combine(_path, "02-3105-DASN10-20100715-01.txt"));
				//var handler = Handler.GetHandler(Path.Combine(_path, "dasn-resumido-001.txt")) ;
				//handler = Handler.GetHandler(@"C:\Temp\_WEBISS\_PROJETOS\ss-importer\DASN\DASN");


				handler = new HandlerDirectory(_path);
				//Path.Combine(_path, "02-3105-DASN10-20100715-01.txt"));

				importer.NotifyLine = true;
				importer.RegisterObserver(view);
			}
			catch (Exception ex)
			{
				var path = AppDomain.CurrentDomain.BaseDirectory;

				using (var file = new StreamWriter(path + "ErroFileHandler.txt", true))
				{
					file.WriteLine(String.Format("\n{0}\n", ex));
				}
			}

			if (handler == null)
				return;

			foreach (var hand in handler.Handlers)
			{
				try
				{
					view.FilePath = hand.Path;

					var bPrint = factoy.GetBlueprint("DASN", hand.FileInfo);

					if (bPrint == null)
					{
						var path = AppDomain.CurrentDomain.BaseDirectory;

						using (var file = new StreamWriter(path + "Versoes.txt", true))
						{
							file.WriteLine("\nVersão: {0} | File: {1}\n", GetVersion(hand.FileInfo), hand.FileInfo.Path);
						}

						continue;
					}


					importer.SetBlueprint(bPrint);
					importer.SetFileToProcess(hand.FileInfo);
					importer.Valid();

					if (importer.IsValid)
						importer.Process();

					importer.Reset();
				}
				catch (Exception ex)
				{
					var path = AppDomain.CurrentDomain.BaseDirectory;

					using (var file = new StreamWriter(path + "Exception.txt", true))
					{
						file.WriteLine("\n{0}\n", ex);
					}
				}
			}

			Console.WriteLine("Pressione qualquer tecla para continuar.");
			Console.ReadKey();
		}
	}
}