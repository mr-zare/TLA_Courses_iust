using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace ConsoleApp
{
    public class Node
    {
        public string nam;
        public List<ertebat> conc = new List<ertebat>();
        public bool fstate;
        public Node(string name)
        {
            nam = name;
        }
        public Node(bool nahaii, string name)
        {
            nam = name;
            fstate = nahaii;
        }
    }
    public class ertebat
    {
        public Node dest;
        public string path;
        public ertebat( Node maqsad, string mabda)
        {
            path = mabda;
            dest = maqsad;
        }
    }

    public class NFA
    {
        public Node navali;
        public List<Node> node = new List<Node>();
        public NFA asli;
        public List<string> harf = new List<string>();

        public NFA(List<string> harfha)
        {
            harf = harfha;
            node = new List<Node>();
            harf.Add("");
        }
        public NFA(string[] ar, string[] chars, string[] nahaii)
        {
            int tar = ar.Length;
            int tchar = chars.Length;
            for (int i = 0; i < tar; i++)
            {
                node.Add(new Node(Array.Exists<string>(nahaii, x => x == ar[i]), ar[i]));
            }
            for (int i = 0; i < tchar; i++)
            {
                harf.Add(chars[i]);
            }
            harf.Add("");
        }

        public void checkNFAanswer(string str)
        {
            bool aa = isNFA(this.node[0], str, str.Length - 1,0);
            if (aa == true)
                Console.WriteLine("Accepted");
            else
                Console.WriteLine("Rejected");
        }
        private bool isNFA(Node jari, string voroodi, int finish,int shoroe)
        {
            if (shoroe > finish)
            {
                if (jari.fstate)
                {
                    return true;
                }
                else
                {
                    bool javab = false;
                    foreach (ertebat conec in jari.conc)
                    {
                        if (conec.path == "")
                        {
                            javab =(javab || isNFA(conec.dest, voroodi, finish, shoroe));
                        }
                    }
                    return javab;
                }
            }
            else
            {
                if (jari.conc.Count == 0)
                {
                    return false;
                }
                else
                {
                    bool javab = false;
                    foreach (ertebat conec in jari.conc)
                    {
                        string a = voroodi[shoroe].ToString();
                        if (conec.path == a)
                        {
                            int sh = shoroe + 1;
                            javab =isNFA(conec.dest,voroodi, finish, sh);
                        }
                        else if (conec.path == "")
                        {
                            javab = (isNFA( conec.dest, voroodi, finish, shoroe) || javab);
                        }
                    }
                    return javab;
                }
            }
        }
        public void addrabete(string[] arr)
        {
            if (arr[1] != "$")
            {
                ertebat a = new ertebat( node.Find(x => x.nam == arr[2]), arr[1]);
                node.Find(x => x.nam == arr[0]).conc.Add(a);
                asli.node.Find(x => x.nam == arr[0]).conc.Add(a);
            }
            else
            {
                ertebat a = new ertebat( node.Find(x => x.nam == arr[2]), "");
                node.Find(x => x.nam == arr[0]).conc.Add(a); ;
                asli.node.Find(x => x.nam == arr[0]).conc.Add((a));
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            string inp1, inp2, inp3;
            inp1 = Console.ReadLine().Trim('{', '}');
            string[] arr = inp1.Split(',');
            inp2 = Console.ReadLine().Trim('{', '}');
            string[] charac = inp2.Split(',');
            inp3 = Console.ReadLine().Trim('{', '}');
            string[] fstates = inp3.Split(',');
            NFA nfa = new NFA(arr, charac, fstates);
            List<string> a = new List<string>();
            nfa.asli = new NFA(a);
            Node qq1 = new Node("dakheli");
            Node qq2 = new Node("nahai");
            nfa.asli.navali =qq1;
            nfa.asli.node.Add(nfa.asli.navali);
            nfa.asli.node.Add(qq2);
            nfa.asli.node[1].fstate = true;
            Node aa = new Node(arr[0]);
            nfa.asli.node.Add(aa);
            ertebat b = new ertebat(nfa.asli.node[2],"");
            nfa.asli.navali.conc.Add(b);
            nfa.asli.harf = new List<string>(charac);
            foreach (string s in arr)
            {
                if (s != "q0")
                {
                    Node s0 = new Node(s);
                    nfa.asli.node.Add(s0);
                    if (Array.Exists<string>(fstates, x => x == s))
                    {
                        ertebat w = new ertebat( nfa.asli.node[1],"");
                        s0.conc.Add(w);
                    }
                }
            }
            int n = int.Parse(Console.ReadLine());
            for (int i = 0; i < n; i++)
            {
                inp1 = Console.ReadLine().Trim('{', '}');
                nfa.addrabete(inp1.Split(','));
            }
            nfa.navali = nfa.node[0];
            nfa.checkNFAanswer(Console.ReadLine());
        }
    }
}
