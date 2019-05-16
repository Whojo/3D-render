using System;
using System.Data;
using System.Globalization;
using System.IO;

namespace Raymarcher
{
    public enum TokenType
    {
        LPARA,
        RPARA,
        INT,
        ID,
        INVALID,
        EOF
    }
    public struct Token
    {
        public TokenType type;
        public string value;//convert as needed

        public Token(TokenType t)
        {
            type = t;
            value = "";
        }
    }
    public class SceneLexer
    {
        private StreamReader stream;
        private Token lastToken;
        public SceneLexer(StreamReader file)
        {
            stream = file;
            lastToken = new Token();
        }

        public Token GetNextToken(bool eat = true)
        {
            if (!eat)
                return lastToken;
            char c = (char)stream.Read();
            while (c == ' ' || c == '\n' || c == '\r')
                c = (char)stream.Read();
            string tok = c.ToString();

            if (c >= '0' && c <= '9' || c == '-')
             {
                return lastToken = BuildToken(tok, '0', '9', TokenType.INT);
            }

            if (c >= 'a' && c <= 'z')
            {
                Token tmp = BuildToken(tok, 'a', 'z', TokenType.ID);

                return lastToken = tmp;
            }

            if (c == '(')
            {
                Token tmp = new Token(TokenType.LPARA);
                return lastToken = tmp;
            }

            if (c == ')')
            {
                Token tmp = new Token(TokenType.RPARA);
                return lastToken = tmp;
            }

            if (c == -1)
            {
                Token tmp = new Token(TokenType.EOF);
                return lastToken = tmp;
            }

            Token invalid = new Token(TokenType.INVALID);
            return lastToken = invalid;
        }

        private Token BuildToken(string tok, int min, int max, TokenType type)
        {
            bool addDot = type == TokenType.INT;
            bool first = true;
            while (stream.Peek() >= min && stream.Peek() <= max || (addDot && (stream.Peek() == '.'
                                                                               || stream.Peek() == ','))
                                                                || (first && addDot && stream.Peek() == '-'))
            {
                tok += (char)(stream.Read());
                first = false;
            }

            Token ret = new Token();
            ret.type = type;
            ret.value = tok;
            return ret;
        }
        
        
    }
    public class SceneParser
    {
        public static Vector3 camPos;
        public static Vector3 camFwd;
        public static float camFov;
        public static Vector3 lightDir;
        public static SDF ParseFile(string filename)
        {
            SDF res;
            using (StreamReader file = new StreamReader(filename))
            {
                try
                {
                    string[] line = file.ReadLine().Split(' ');
                    camPos = new Vector3(float.Parse(line[0], NumberStyles.Any, CultureInfo.InvariantCulture),
                        float.Parse(line[1], NumberStyles.Any, CultureInfo.InvariantCulture),
                        float.Parse(line[2], NumberStyles.Any, CultureInfo.InvariantCulture));
                    line = file.ReadLine().Split(' ');
                    camFwd = new Vector3(float.Parse(line[0], NumberStyles.Any, CultureInfo.InvariantCulture),
                        float.Parse(line[1], NumberStyles.Any, CultureInfo.InvariantCulture),
                        float.Parse(line[2], NumberStyles.Any, CultureInfo.InvariantCulture));
                    string tmp = file.ReadLine();
                    camFov = float.Parse(tmp, NumberStyles.Any, CultureInfo.InvariantCulture);
                    line = file.ReadLine().Split(' ');
                    lightDir = new Vector3(float.Parse(line[0], NumberStyles.Any, CultureInfo.InvariantCulture),
                        float.Parse(line[1], NumberStyles.Any, CultureInfo.InvariantCulture),
                        float.Parse(line[2], NumberStyles.Any, CultureInfo.InvariantCulture));
                }
                catch (Exception)
                {
                    throw new InvalidDataException("Malformed header in input file");
                }

                file.ReadLine();
                
                SceneLexer lexer = new SceneLexer(file);
                lexer.GetNextToken();
                res = ParseNode(lexer);
            }
            
            return res;
        }

        private static SDF ParseNode(SceneLexer lexer)
        {
            if (lexer.GetNextToken(false).type != TokenType.LPARA)
                throw new InvalidDataException("Invalid file format: Expected LPARA token.");
            lexer.GetNextToken();
            if (lexer.GetNextToken(false).type != TokenType.ID)
                throw new InvalidDataException("Invalid file format: Expected ID token.");

            NodeData node = new NodeData();
            node.name = lexer.GetNextToken(false).value;
            lexer.GetNextToken();
            while (lexer.GetNextToken(false).type == TokenType.INT)
            {
                //add parameters to list
                node.args.Add(float.Parse(lexer.GetNextToken(false).value,
                    NumberStyles.Any, CultureInfo.InvariantCulture));
                lexer.GetNextToken();
            }

            if (lexer.GetNextToken(false).type == TokenType.LPARA)
            {
                node.lChild = ParseNode(lexer);
            }

            if (lexer.GetNextToken(false).type == TokenType.LPARA)
            {
                node.rChild = ParseNode(lexer);
            }
            
            if (lexer.GetNextToken(false).type != TokenType.RPARA)
                throw new InvalidDataException("Invalid file format: Expected RPARA token.");
            lexer.GetNextToken();

            return NodeFactory.CreateNode(node);
        }
    }
}