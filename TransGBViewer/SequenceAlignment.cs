using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransGBViewer
{
    public class SequenceAlignment
    {
        private enum Direction { Diagonal, Up, Left, None }

        public static (string alignedSeq1, string alignedSeq2) GlobalDNAAlignment(string seq1, string seq2)
        {
            int matchScore = 1;
            int mismatchPenalty = -1;
            int gapPenalty = -5;

            int m = seq1.Length;
            int n = seq2.Length;
            int[,] score = new int[m + 1, n + 1];
            Direction[,] traceback = new Direction[m + 1, n + 1];

            // Initialize scoring matrix
            for (int i = 0; i <= m; i++)
            {
                score[i, 0] = i * gapPenalty;
                traceback[i, 0] = Direction.Up;
            }
            for (int j = 0; j <= n; j++)
            {
                score[0, j] = j * gapPenalty;
                traceback[0, j] = Direction.Left;
            }

            // Fill scoring matrix
            for (int i = 1; i <= m; i++)
            {
                for (int j = 1; j <= n; j++)
                {
                    int match = score[i - 1, j - 1] + (seq1[i - 1] == seq2[j - 1] ? matchScore : mismatchPenalty);
                    int delete = score[i - 1, j] + gapPenalty;
                    int insert = score[i, j - 1] + gapPenalty;

                    score[i, j] = Math.Max(match, Math.Max(delete, insert));

                    if (score[i, j] == match)
                        traceback[i, j] = Direction.Diagonal;
                    else if (score[i, j] == delete)
                        traceback[i, j] = Direction.Up;
                    else
                        traceback[i, j] = Direction.Left;
                }
            }

            // Traceback
            StringBuilder aligned1 = new StringBuilder();
            StringBuilder aligned2 = new StringBuilder();
            int x = m, y = n;

            while (x > 0 || y > 0)
            {
                if (traceback[x, y] == Direction.Diagonal)
                {
                    aligned1.Insert(0, seq1[x - 1]);
                    aligned2.Insert(0, seq2[y - 1]);
                    x--; y--;
                }
                else if (traceback[x, y] == Direction.Up)
                {
                    aligned1.Insert(0, seq1[x - 1]);
                    aligned2.Insert(0, '-');
                    x--;
                }
                else
                {
                    aligned1.Insert(0, '-');
                    aligned2.Insert(0, seq2[y - 1]);
                    y--;
                }
            }

            return (aligned1.ToString(), aligned2.ToString());
        }

        public static (string alignedSeq1, string alignedSeq2, int score) LocalDNAAlignment(string seq1, string seq2)
        {
            int matchScore = 2;
            int mismatchPenalty = -1;
            int gapPenalty = -5;

            int m = seq1.Length;
            int n = seq2.Length;
            int[,] score = new int[m + 1, n + 1];
            Direction[,] traceback = new Direction[m + 1, n + 1];

            int maxScore = 0;
            int maxI = 0, maxJ = 0;

            // Fill scoring matrix
            for (int i = 1; i <= m; i++)
            {
                for (int j = 1; j <= n; j++)
                {
                    int match = score[i - 1, j - 1] + (seq1[i - 1] == seq2[j - 1] ? matchScore : mismatchPenalty);
                    int delete = score[i - 1, j] + gapPenalty;
                    int insert = score[i, j - 1] + gapPenalty;

                    int best = Math.Max(0, Math.Max(match, Math.Max(delete, insert)));
                    score[i, j] = best;

                    if (best == 0) traceback[i, j] = Direction.None;
                    else if (best == match) traceback[i, j] = Direction.Diagonal;
                    else if (best == delete) traceback[i, j] = Direction.Up;
                    else traceback[i, j] = Direction.Left;

                    if (best > maxScore)
                    {
                        maxScore = best;
                        maxI = i;
                        maxJ = j;
                    }
                }
            }

            // Traceback from highest scoring cell
            StringBuilder aligned1 = new StringBuilder();
            StringBuilder aligned2 = new StringBuilder();
            int x = maxI, y = maxJ;

            while (x > 0 && y > 0 && score[x, y] > 0)
            {
                if (traceback[x, y] == Direction.Diagonal)
                {
                    aligned1.Insert(0, seq1[x - 1]);
                    aligned2.Insert(0, seq2[y - 1]);
                    x--; y--;
                }
                else if (traceback[x, y] == Direction.Up)
                {
                    aligned1.Insert(0, seq1[x - 1]);
                    aligned2.Insert(0, '-');
                    x--;
                }
                else if (traceback[x, y] == Direction.Left)
                {
                    aligned1.Insert(0, '-');
                    aligned2.Insert(0, seq2[y - 1]);
                    y--;
                }
                else break;
            }

            return (aligned1.ToString(), aligned2.ToString(), maxScore);
        }

        public static (string Seq1CommonRegion, string Seq2CommonRegion) BlockedAlignment(string seq1, string seq2)
        {
            string Bit1 = "";
            string Bit2 = "";
            int offset = 1;
            string common = seq1.Substring(offset, 20);
            int place = seq2.IndexOf(common);
            bool run = true;
            while (run == true)
            {
                string bit1 = "";
                string bit2 = "";
                if (place > 1)
                {
                    (bit1, bit2) = extend(common, offset, place, seq1, seq2);
                    if (bit1.Length > Bit1.Length)
                    {
                        Bit1 = bit1;
                        Bit2 = bit2;
                    }
                    offset += bit1.Length - common.Length + 1;
                }
                else { offset++; }

                if (offset + 20 > seq1.Length)
                { run = false; }
                else
                {
                    common = seq1.Substring(offset, 20);
                    place = seq2.IndexOf(common);
                }
            }
            return (Bit1, Bit2);
        }

        private static (string seq1Bit, string seq2Bit) extend(string common, int PlaceIn1, int PlaceIn2, string seq1, string seq2)
        {
            int extra5 = 5;
            int placeIn1 = PlaceIn1 + 4;
            int placeIn2 = PlaceIn2 + 4;
            int commonlength = common.Length;
            if (placeIn1 > 5 && placeIn2 > 5)
            {
                bool run = true;
                while (run)
                {
                    if (placeIn1 - extra5 < 0 || placeIn2 - extra5 < 0)
                    {
                        run = false;
                        extra5--;
                    }
                    else
                    {
                        string one = seq1.Substring(placeIn1 - extra5, extra5);
                        string two = seq2.Substring(placeIn2 - extra5, extra5);

                        if (one == two)
                        {
                            if (extra5 == 10)
                            { placeIn2--; placeIn1--; }
                            else { extra5++; }
                        }
                        else
                        {
                            int counter = 0;
                            for (int index = 0; index < extra5; index++)
                            {
                                if (seq1[placeIn1 - index] == seq2[placeIn2 - index])
                                { counter++; }
                            }

                            if (extra5 - counter == 1 || (float)counter / extra5 > 0.89f)
                            {
                                if (extra5 == 10)
                                { placeIn2--; placeIn1--; }
                                else { extra5++; }
                            }
                            else
                            { extra5--; }
                        }
                    }
                }
            }

            int startPoint1 = placeIn1 - extra5;
            int startPoint2 = placeIn2 - extra5;

            int extra3 = 5;            
            placeIn1 = PlaceIn1 + commonlength - 4;
            placeIn2 = PlaceIn2 + commonlength - 4;
            if (placeIn1 + extra3 < seq1.Length && placeIn2 + extra3 < seq2.Length)
            {
                bool run = true;
                while (run)
                {
                    if (placeIn1 + extra3 > seq1.Length || placeIn2 + extra3 > seq2.Length)
                    { 
                        run = false;
                        extra3--;
                    }
                    else
                    {
                        string one = seq1.Substring(placeIn1, extra3);
                        string two = seq2.Substring(placeIn2, extra3);
                        
                        if (one == two)
                        {
                            if (extra3 == 10)
                            { placeIn2++; placeIn1++; }
                            else { extra3++; }
                        }
                        else
                        {
                            int counter = 0;
                            for (int index = 0; index < extra3; index++)
                            {
                                if (seq1[placeIn1 + commonlength + index] == seq2[placeIn2 + commonlength + index])
                                { counter++; }
                            }
                        
                        if (extra3 - counter == 1 || (float)counter / extra3 > 0.89f)
                        {
                            if (extra3 == 10)
                            { placeIn2++; placeIn1++; }
                            else { extra3++; }
                        }
                        else
                        { extra3--; }
                        }
                    }
                }
            }

            int endPoint1 = placeIn1 + extra3;
            int endPoint2 = placeIn2 + extra3;
            string bit1 = seq1.Substring(startPoint1, endPoint1 - startPoint1 );
            string bit2 = seq2.Substring(startPoint2, endPoint2 - startPoint2 );


            return (bit1, bit2);
        }
               
        public static (string aligned1, string aligned2) AlignProteinGlobal(string seq1, string seq2)
        {
            int proteingapPenalty = -4;
            Dictionary<(char, char), int> blosum62 = LoadBlosum62();


            int m = seq1.Length;
            int n = seq2.Length;
            int[,] score = new int[m + 1, n + 1];
            Direction[,] traceback = new Direction[m + 1, n + 1];

            // Initialize
            for (int i = 0; i <= m; i++)
            {
                score[i, 0] = i * proteingapPenalty;
                traceback[i, 0] = Direction.Up;
            }
            for (int j = 0; j <= n; j++)
            {
                score[0, j] = j * proteingapPenalty;
                traceback[0, j] = Direction.Left;
            }

            // Fill matrix
            for (int i = 1; i <= m; i++)
            {
                for (int j = 1; j <= n; j++)
                {
                    int subScore = blosum62.TryGetValue((seq1[i - 1], seq2[j - 1]), out int s) ? s : -1;
                    int match = score[i - 1, j - 1] + subScore;
                    int delete = score[i - 1, j] + proteingapPenalty;
                    int insert = score[i, j - 1] + proteingapPenalty;

                    score[i, j] = Math.Max(match, Math.Max(delete, insert));

                    if (score[i, j] == match)
                        traceback[i, j] = Direction.Diagonal;
                    else if (score[i, j] == delete)
                        traceback[i, j] = Direction.Up;
                    else
                        traceback[i, j] = Direction.Left;
                }
            }

            // Traceback
            StringBuilder aligned1 = new StringBuilder();
            StringBuilder aligned2 = new StringBuilder();
            int x = m, y = n;

            while (x > 0 || y > 0)
            {
                if (traceback[x, y] == Direction.Diagonal)
                {
                    aligned1.Insert(0, seq1[x - 1]);
                    aligned2.Insert(0, seq2[y - 1]);
                    x--; y--;
                }
                else if (traceback[x, y] == Direction.Up)
                {
                    aligned1.Insert(0, seq1[x - 1]);
                    aligned2.Insert(0, '-');
                    x--;
                }
                else
                {
                    aligned1.Insert(0, '-');
                    aligned2.Insert(0, seq2[y - 1]);
                    y--;
                }
            }

            return (aligned1.ToString(), aligned2.ToString());
        }

        public static (string aligned1, string aligned2) AlignProteinLocal(string seq1, string seq2)
        {
            int proteingapPenalty = -4;
            Dictionary<(char, char), int> blosum62 = LoadBlosum62();

            int m = seq1.Length;
            int n = seq2.Length;
            int[,] score = new int[m + 1, n + 1];
            Direction[,] traceback = new Direction[m + 1, n + 1];

            // Initialize to 0 for local alignment
            for (int i = 0; i <= m; i++)
            {
                score[i, 0] = 0;
                traceback[i, 0] = Direction.None;
            }
            for (int j = 0; j <= n; j++)
            {
                score[0, j] = 0;
                traceback[0, j] = Direction.None;
            }

            // Fill matrix
            int maxScore = 0;
            int maxI = 0, maxJ = 0;

            for (int i = 1; i <= m; i++)
            {
                for (int j = 1; j <= n; j++)
                {
                    int subScore = blosum62.TryGetValue((seq1[i - 1], seq2[j - 1]), out int s) ? s : -1;
                    int match = score[i - 1, j - 1] + subScore;
                    int delete = score[i - 1, j] + proteingapPenalty;
                    int insert = score[i, j - 1] + proteingapPenalty;

                    int best = Math.Max(0, Math.Max(match, Math.Max(delete, insert)));
                    score[i, j] = best;

                    if (best == 0)
                        traceback[i, j] = Direction.None;
                    else if (best == match)
                        traceback[i, j] = Direction.Diagonal;
                    else if (best == delete)
                        traceback[i, j] = Direction.Up;
                    else
                        traceback[i, j] = Direction.Left;

                    if (best > maxScore)
                    {
                        maxScore = best;
                        maxI = i;
                        maxJ = j;
                    }
                }
            }

            // Traceback from max score
            StringBuilder aligned1 = new StringBuilder();
            StringBuilder aligned2 = new StringBuilder();
            int x = maxI, y = maxJ;

            while (x > 0 && y > 0 && traceback[x, y] != Direction.None)
            {
                if (traceback[x, y] == Direction.Diagonal)
                {
                    aligned1.Insert(0, seq1[x - 1]);
                    aligned2.Insert(0, seq2[y - 1]);
                    x--; y--;
                }
                else if (traceback[x, y] == Direction.Up)
                {
                    aligned1.Insert(0, seq1[x - 1]);
                    aligned2.Insert(0, '-');
                    x--;
                }
                else // Left
                {
                    aligned1.Insert(0, '-');
                    aligned2.Insert(0, seq2[y - 1]);
                    y--;
                }
            }

            return (aligned1.ToString(), aligned2.ToString());
        }

        private static Dictionary<(char, char), int> LoadBlosum62()
        {
            var matrix = new Dictionary<(char, char), int>();
            string[] lines = {
                "A R N D C Q E G H I L K M F P S T W Y V",
                "A 4 -1 -2 -2 0 -1 -1 0 -2 -1 -1 -1 -1 -2 -1 1 0 -3 -2 0",
                "R -1 5 0 -2 -3 1 0 -2 0 -3 -2 2 -1 -3 -2 -1 -1 -3 -2 -3",
                "N -2 0 6 1 -3 0 0 0 1 -3 -3 0 -2 -3 -2 1 0 -4 -2 -3",
                "D -2 -2 1 6 -3 0 2 -1 -1 -3 -4 -1 -3 -3 -1 0 -1 -4 -3 -3",
                "C 0 -3 -3 -3 9 -3 -4 -3 -3 -1 -1 -3 -1 -2 -3 -1 -1 -2 -2 -1",
                "Q -1 1 0 0 -3 5 2 -2 0 -3 -2 1 0 -3 -1 0 -1 -2 -1 -2",
                "E -1 0 0 2 -4 2 5 -2 0 -3 -3 1 -2 -3 -1 0 -1 -3 -2 -2",
                "G 0 -2 0 -1 -3 -2 -2 6 -2 -4 -4 -2 -3 -3 -2 0 -2 -2 -3 -3",
                "H -2 0 1 -1 -3 0 0 -2 8 -3 -3 -1 -2 -1 -2 -1 -2 -2 2 -3",
                "I -1 -3 -3 -3 -1 -3 -3 -4 -3 4 2 -3 1 0 -3 -2 -1 -3 -1 3",
                "L -1 -2 -3 -4 -1 -2 -3 -4 -3 2 4 -2 2 0 -3 -2 -1 -2 -1 1",
                "K -1 2 0 -1 -3 1 1 -2 -1 -3 -2 5 -1 -3 -1 0 -1 -3 -2 -2",
                "M -1 -1 -2 -3 -1 0 -2 -3 -2 1 2 -1 5 0 -2 -1 -1 -1 -1 1",
                "F -2 -3 -3 -3 -2 -3 -3 -3 -1 0 0 -3 0 6 -4 -2 -2 1 3 -1",
                "P -1 -2 -2 -1 -3 -1 -1 -2 -2 -3 -3 -1 -2 -4 7 -1 -1 -4 -3 -2",
                "S 1 -1 1 0 -1 0 0 0 -1 -2 -2 0 -1 -2 -1 4 1 -3 -2 -2",
                "T 0 -1 0 -1 -1 -1 -1 -2 -2 -1 -1 -1 -1 -2 -1 1 5 -2 -2 0",
                "W -3 -3 -4 -4 -2 -2 -3 -2 -2 -3 -2 -3 -1 1 -4 -3 -2 11 2 -3",
                "Y -2 -2 -2 -3 -2 -1 -2 -3 2 -1 -1 -2 -1 3 -3 -2 -2 2 7 -1",
                "V 0 -3 -3 -3 -1 -2 -2 -3 -3 3 1 -2 1 -1 -2 -2 0 -3 -1 4"
            };

            var aa = lines[0].Split(' ');
            for (int i = 1; i < lines.Length; i++)
            {
                var tokens = lines[i].Split(' ');
                char row = tokens[0][0];
                for (int j = 1; j < tokens.Length; j++)
                {
                    char col = aa[j - 1][0];
                    int val = int.Parse(tokens[j]);
                    matrix[(row, col)] = val;
                }
            }

            return matrix;
        }
    }
}
