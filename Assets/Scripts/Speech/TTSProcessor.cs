using System.Collections.Generic;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace SmartAssistant.Speech.TTS
{
  public partial class TextToSpeech
  {
    // private static readonly string[] _arpabet = new string[]
    // {
    //   "@AA", "@AA0", "@AA1", "@AA2", "@AE", "@AE0", "@AE1", "@AE2", "@AH", "@AH0", "@AH1", "@AH2",
    //   "@AO", "@AO0", "@AO1", "@AO2", "@AW", "@AW0", "@AW1", "@AW2", "@AY", "@AY0", "@AY1", "@AY2",
    //   "@B",
    //   "@CH",
    //   "@D", "@DH",
    //   "@EH", "@EH0", "@EH1", "@EH2", "@ER", "@ER0", "@ER1", "@ER2", "@EY", "@EY0", "@EY1", "@EY2",
    //   "@F",
    //   "@G",
    //   "@HH",
    //   "@IH", "@IH0", "@IH1", "@IH2", "@IY", "@IY0", "@IY1", "@IY2",
    //   "@JH",
    //   "@K",
    //   "@L",
    //   "@M",
    //   "@N", "@NG",
    //   "@OW", "@OW0", "@OW1", "@OW2", "@OY", "@OY0", "@OY1", "@OY2",
    //   "@P",
    //   "@R",
    //   "@S", "@SH",
    //   "@T", "@TH",
    //   "@UH", "@UH0", "@UH1", "@UH2", "@UW", "@UW0", "@UW1", "@UW2",
    //   "@V",
    //   "@W",
    //   "@Y",
    //   "@Z",
    //   "@ZH"
    // };
    // private static readonly string _pad = "pad";
    // private static readonly string _eos = "eos";
    // private static readonly string _punctuation = "!'(),.:;? ";
    // private static readonly string _special = "-";
    // private static readonly string _letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

    public string mapperFilepath;

    private static readonly Regex _curlyBracesRegex = new Regex(@"(.*?)\{(.+?)\}(.*)");

    private void InitTTSProcessor()
    {
      // JsonSerializer
    }

    /// <summary>
    /// Perform preprocessing and raw feature extraction for LJSpeech dataset
    /// </summary>
    /// <param name="text">input text</param>
    /// <returns></returns>
    private int[] TextToSequence(string text)
    {
      List<int> sequence = new List<int>();

      while (text.Length > 0)
      {
        Match match = _curlyBracesRegex.Match(text);
        if (!match.Success)
        {
          sequence.AddRange(_SymbolsToSequence(text));
          break;
        }

        sequence.AddRange(_SymbolsToSequence(match.Groups[1].Value));
        sequence.AddRange(_ArpabetToSequence(match.Groups[2].Value));
        text = match.Groups[3].Value;
      }

      // sequence.Add(_eosID)
      return sequence.ToArray();
    }

    private static List<int> _SymbolsToSequence(string symbols)
    {
      List<int> outputSequence = new List<int>();

      return outputSequence;
    }

    private static List<int> _ArpabetToSequence(string text)
    {
      List<int> outputSequence = new List<int>();

      return outputSequence;
    }

    private static bool _ShouldKeepSymbol(ref char symbol)
    {
      return symbol != '_' && symbol != '~';
    }
  }
}