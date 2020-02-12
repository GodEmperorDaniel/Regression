// This code is part of the Fungus library (http://fungusgames.com) maintained by Chris Gregan (http://twitter.com/gofungus).
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Fungus
{
    /// <summary>
    /// Parses a string for special Fungus text tags.
    /// </summary>
    public static class TextTagParser
    {
        const string TextTokenRegexString = @"\{.*?\}";

        private static void AddWordsToken(List<TextTagToken> tokenList, string words)
        {
            TextTagToken token = new TextTagToken();
            token.type = TokenType.Words;
            token.paramList = new List<string>(); 
            token.paramList.Add(words);
            tokenList.Add(token);
        }
        
        private static void AddTagToken(List<TextTagToken> tokenList, string tagText)
        {
            if (tagText.Length < 3 ||
                tagText.Substring(0,1) != "{" ||
                tagText.Substring(tagText.Length - 1,1) != "}")
            {
                return;
            }
            
            string tag = tagText.Substring(1, tagText.Length - 2);
            
            var type = TokenType.Invalid;
            List<string> parameters = ExtractParameters(tag);
            
            if (tag == "b")
            {
                type = TokenType.BoldStart;
            }
            else if (tag == "/b")
            {
                type = TokenType.BoldEnd;
            }
            else if (tag == "i")
            {
                type = TokenType.ItalicStart;
            }
            else if (tag == "/i")
            {
                type = TokenType.ItalicEnd;
			} else if (tag.StartsWith("color=")) {
				type = TokenType.ColorStart;
			} else if (tag == "/color") {
				type = TokenType.ColorEnd;
			} else if (tag.StartsWith("hcolor=") || tag.StartsWith("hColor=")) {
				type = TokenType.HiddenColorStart;
			} else if (tag == "/hcolor" || tag.StartsWith("/hColor")) {
				type = TokenType.HiddenColorEnd;
			} else if (tag.StartsWith("size="))
            {
                type = TokenType.SizeStart;
            }
            else if (tag == "/size")
            {
                type = TokenType.SizeEnd;
            }
            else if (tag == "wi")
            {
                type = TokenType.WaitForInputNoClear;
            }
            else if (tag == "wc")
            {
                type = TokenType.WaitForInputAndClear;
            }
            else if (tag == "wvo")
            {
                type = TokenType.WaitForVoiceOver;
            }
            else if (tag.StartsWith("wp="))
            {
                type = TokenType.WaitOnPunctuationStart;
            }
            else if (tag == "wp")
            {
                type = TokenType.WaitOnPunctuationStart;
            }
            else if (tag == "/wp")
            {
                type = TokenType.WaitOnPunctuationEnd;
            }
            else if (tag.StartsWith("w="))
            {
                type = TokenType.Wait;
            }
            else if (tag == "w")
            {
                type = TokenType.Wait;
            }
            else if (tag == "c")
            {
                type = TokenType.Clear;
            }
            else if (tag.StartsWith("speed="))
            {
                type = TokenType.SpeedStart;
            }
            else if (tag == "speed")
            {
                type = TokenType.SpeedStart;
            }
            else if (tag == "/speed")
            {
                type = TokenType.SpeedEnd;
            }
            else if (tag == "x")
            {
                type = TokenType.Exit;
            }
            else if (tag.StartsWith("m="))
            {
                type = TokenType.Message;
            }
            else if (tag.StartsWith("vpunch") ||
                     tag.StartsWith("vpunch="))
            {
                type = TokenType.VerticalPunch;
            }
            else if (tag.StartsWith("hpunch") ||
                     tag.StartsWith("hpunch="))
            {
                type = TokenType.HorizontalPunch;
            }
            else if (tag.StartsWith("punch") ||
                     tag.StartsWith("punch="))
            {
                type = TokenType.Punch;
            }
            else if (tag.StartsWith("flash") ||
                     tag.StartsWith("flash="))
            {
                type = TokenType.Flash;
            }
            else if (tag.StartsWith("audio="))
            {
                type = TokenType.Audio;
            }
            else if (tag.StartsWith("audioloop="))
            {
                type = TokenType.AudioLoop;
            }
            else if (tag.StartsWith("audiopause="))
            {
                type = TokenType.AudioPause;
			}

			if (type == TokenType.Invalid && tag != "") {
				foreach (var tmp_tag in TMP_Tag.allTags) {

					if ((tmp_tag.parameterCount > 0)? tag.StartsWith(tmp_tag.open) : tag == tmp_tag.open) {
						type = TokenType.TMProTag;
						parameters.Insert(0, tmp_tag.open);
						parameters.Insert(1, tmp_tag.shouldHide.ToString());
						break;
					} else if (tag == tmp_tag.close) {
						type = TokenType.TMProTag;
						parameters.Insert(0, tmp_tag.close);
						parameters.Insert(1, tmp_tag.shouldHide.ToString());
						break;
					}
				}
			}

			if (type != TokenType.Invalid)
            {
                TextTagToken token = new TextTagToken();
                token.type = type;
                token.paramList = parameters;           
                tokenList.Add(token);
            }
            else
            {
                Debug.LogWarning("Invalid text tag '" + tag + "'");
            }
        }

        private static List<string> ExtractParameters(string input)
        {
            List<string> paramsList = new List<string>();
            int index = input.IndexOf('=');
            if (index == -1)
            {
                return paramsList;
            }

            string paramsStr = input.Substring(index + 1);
            var splits = paramsStr.Split(',');
            for (int i = 0; i < splits.Length; i++)
            {
                var p = splits[i];
                paramsList.Add(p.Trim());
            }
            return paramsList;
        }

        #region Public members

        /// <summary>
        /// Returns a description of the supported tags.
        /// </summary>
        public static string GetTagHelp()
        {
			return "" +
				"{b} Bold Text {/b}\n" +
				"{i} Italic Text {/i}\n" +
				"{color=red} Color Text (color){/color}\n" +
				"{size=30} Text size {/size}\n" +
				"\n" +
				"{speed}, {speed=60} Writing speed (chars per sec){/speed}\n" +
				"{w}, {w=0.5} Wait (seconds)\n" +
				"{wi} Wait for input\n" +
				"{wc} Wait for input and clear\n" +
				"{wvo} Wait for voice over line to complete\n" +
				"{wp}, {wp=0.5} Wait on punctuation (seconds){/wp}\n" +
				"{c} Clear\n" +
				"{x} Exit, advance to the next command without waiting for input\n" +
				"\n" +
				"{vpunch=10,0.5} Vertically punch screen (intensity,time)\n" +
				"{hpunch=10,0.5} Horizontally punch screen (intensity,time)\n" +
				"{punch=10,0.5} Punch screen (intensity,time)\n" +
				"{flash=0.5} Flash screen (duration)\n" +
				"\n" +
				"{audio=AudioObjectName} Play Audio Once\n" +
				"{audioloop=AudioObjectName} Play Audio Loop\n" +
				"{audiopause=AudioObjectName} Pause Audio\n" +
				"{audiostop=AudioObjectName} Stop Audio\n" +
				"\n" +
				"{m=MessageName} Broadcast message\n" +
				"{$VarName} Substitute variable\n" +
				"{hcolor=} Color Hidden Text{/hcolor}\n" +
				"\n" +
				"//TMPro Tags//\n" +
				"\n* Can be in font units (em) or pixels (px)\n" +
				"** Can be in font units (em), pixels (px), or percentages (%)\n" +
				"\n" +
				"{align=} Text alignment {/align} Can be \"left\", \"center\", \"right\", \"justified\", or \"flush\"\n" +
				"{cspace=1em} Spacing between characters {/cspace} * Can be negative\n" +
				"{indent=10%} Horizontal Offset that is persistent between lines {/indent} **\n" +
				"{line-height=50%} Line Height {/line-height} **\n" +
				"{line-indent=10px} Indentation that doesn't affect word-wrapped lines {/line-indent} **\n" +
				"{link=\"ID\"} Link Metadata {/link}\n" +
				"{font=\"\"} Change Font {/font}\n" +
				"\n" +
				"{lowercase} Lowercase Letters {/lowercase}\n" +
				"{uppercase} Uppercase Letters {/uppercase}\n" +
				"{smallcaps} Capitalizes all lowercase letters and decreases their size {/smallcaps}\n" +
				"{margin=5em} Margin {/margin} **\n" +
				"{margin-left=5em} Left Margin {/margin} **\n" +
				"{margin-right=5em} Right Margin {/margin} **\n" +
				"\n" +
				"{mark=#ffff00aa} Mark Text (color){/mark}\n" +
				"{mspace=2.75em} Monospace Font {/mspace} *\n" +
				//{noparse} No Parse {/noparse}\n" +
				"{nobr} Prevents words from being separated by word wrapping {/nobr}\n" +
				"{page} Insert page break (Requires page overflow mode)\n" +
				"{pos=50%} Set horizontal position to write from, doesn't persist between lines **\n" +
				"{space=2.75em} Inserts Space *\n" +
				"\n" +
				"{sprite=\"assetName\",index=1,color=#FFFFFF} Inserts sprite from asset file with index and optional color\n" +
				"{sprite=\"assetName\",name=\"spriteName\",color=#FFFFFF} Inserts sprite from asset file with name and optional color\n" +
				"{sprite index=1,color=#FFFFFF} Inserts sprite from the default asset file with index and optional color\n" +
				"{sprite name=\"spriteName\",color=#FFFFFF} Inserts sprite from the default asset file with name and optional color\n" +
				"\n" +
				"{s} Strikethrough {/s}\n" +
				"{u} Underline {/u}\n" +
				"{style=\"Style\"} Inserts A Preset Style</style>\n" +
				"{sup} Superscript {/sup}\n" +
				"{sub} Subscript {/sub}\n" +
				"{voffset=0.5em} Offset Text Vertically {/voffset} *\n" +
				"{width=50%} Width of Text Area {/width} **\n" +


				"";
		}

        /// <summary>
        /// Processes a block of story text and converts it to a list of tokens.
        /// </summary>
        public static List<TextTagToken> Tokenize(string storyText)
        {
            List<TextTagToken> tokens = new List<TextTagToken>();

            Regex myRegex = new Regex(TextTokenRegexString);

            Match m = myRegex.Match(storyText);   // m is the first match

            int position = 0;
            while (m.Success)
            {
                // Get bit leading up to tag
                string preText = storyText.Substring(position, m.Index - position);
                string tagText = m.Value;

                if (preText != "")
                {
                    AddWordsToken(tokens, preText);
                }
                AddTagToken(tokens, tagText);

                position = m.Index + tagText.Length;
                m = m.NextMatch();
            }

            if (position < storyText.Length)
            {
                string postText = storyText.Substring(position, storyText.Length - position);
                if (postText.Length > 0)
                {
                    AddWordsToken(tokens, postText);
                }
            }

            // Remove all leading whitespace & newlines after a {c} or {wc} tag
            // These characters are usually added for legibility when editing, but are not 
            // desireable when viewing the text in game.
            bool trimLeading = false;
            for (int i = 0; i < tokens.Count; i++)
            {
                var token = tokens[i];
                if (trimLeading && token.type == TokenType.Words)
                {
                    token.paramList[0] = token.paramList[0].TrimStart(' ', '\t', '\r', '\n');
                }
                if (token.type == TokenType.Clear || token.type == TokenType.WaitForInputAndClear)
                {
                    trimLeading = true;
                }
                else
                {
                    trimLeading = false;
                }
            }

            return tokens;
        }

        #endregion
    }    
}