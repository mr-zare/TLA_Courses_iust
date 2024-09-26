using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace ConsoleApp1
{
    public class NodeNFA
    {
        public List<NFArabete> rabete = new List<NFArabete>();
        public string nam;
        public bool fstate;
        public NodeNFA(string name, bool nahaii)
        {
            nam = name;
            fstate = nahaii;
        }
        public NodeNFA(string name)
        {
            nam = name;
        }

    }
    public class NFArabete
    {
        public string path;
        public NodeNFA des;
        public NFArabete(string masir, NodeNFA maqsad)
        {
            this.path = masir;
            this.des = maqsad;
        }

    }

    public class NFA
    {
        public List<NodeNFA> node = new List<NodeNFA>();
        public List<string> harf = new List<string>();
        public NFA NfAasli;
        public NodeNFA Nodedakheli;
        public NFA(List<string> alphas)
        {
            node = new List<NodeNFA>();
            harf = alphas;
            harf.Add("");
        }
        public NFA(string[] ar, string[] charac, string[] nahaii)
        {
            int tar = ar.Length;
            int tchar = charac.Length;
            for (int i = 0; i < tar; i++)
            {
                NodeNFA a = new NodeNFA(ar[i], Array.Exists<string>(nahaii, x => x == ar[i]));
                node.Add(a);
            }
            for (int i = 0; i < tchar; i++)
            {
                harf.Add(charac[i]);
            }
            harf.Add("");

        }
        public void addrabete(string[] ar)
        {
            if (ar[1] != "$")
            {
                NFArabete a = new NFArabete(ar[1], node.Find(x => x.nam == ar[2]));
                node.Find(x => x.nam == ar[0]).rabete.Add(a);
                NfAasli.node.Find(x => x.nam == ar[0]).rabete.Add(a);
            }
            else
            {
                NFArabete a = new NFArabete("", node.Find(x => x.nam == ar[2]));
                node.Find(x => x.nam == ar[0]).rabete.Add(a);
                NfAasli.node.Find(x => x.nam == ar[0]).rabete.Add(a);
            }
        }
        public int convNFAtoDFA()
        {
            DFA dfa = new DFA(Nodedakheli, harf);
            NFAtoDFA(dfa.statedakheli, dfa);
            for (int i = 0; i < dfa.nodes.Count; i++)
            {
                dfa.nodes[i].nam = "G" + i;
            }
            return dfa.nodes.Count;
        }

        public void NFAtoDFA(NodeDFA a, DFA dfa)
        {
            int ta = a.node.Count;
            for (int i = 0; i < ta; i++)
            {
                int tra = a.node[i].rabete.Count;
                for (int j = 0; j < tra; j++)
                {
                    if (a.node[i].rabete[j].path == "")
                    {
                        if (a.mojood(a.node[i].rabete[j].des)==false)
                        {
                            a.node.Add(a.node[i].rabete[j].des);
                        }
                    }
                }
            }
            foreach (string strin in harf)
            {
                if (strin != "")
                {
                    bool b = true;
                    NodeDFA dfastate = new NodeDFA(harf);
                    foreach (NodeNFA n in a.node)
                    {
                        foreach (NFArabete c in n.rabete)
                        {
                            if (c.path == strin)
                            {
                                b = false;
                                if (dfastate.mojood(c.des)==false)
                                {
                                    dfastate.add(c.des);
                                }
                            }
                        }
                    }
                    int t = dfastate.node.Count;
                    for (int i = 0; i <t ; i++)
                    {
                        int tt = dfastate.node[i].rabete.Count;
                        for (int j = 0; j <tt ; j++)
                        {
                            if (dfastate.node[i].rabete[j].path == "")
                            {
                                if (dfastate.mojood(dfastate.node[i].rabete[j].des)==false)
                                {
                                    dfastate.node.Add(dfastate.node[i].rabete[j].des);
                                }
                            }
                        }
                    }
                    if (b==true)
                    {
                        a.rabete.Add(new DFArabete(dfa.node1, strin));
                    }
                    else
                    {
                        if (dfa.mojooddfa(dfastate) == null)
                        {

                            dfa.nodes.Add(dfastate);
                            a.rabete.Add(new DFArabete(dfastate, strin));
                            NFAtoDFA(dfastate, dfa);
                        }
                        else
                        {
                            a.rabete.Add(new DFArabete(dfa.mojooddfa(dfastate), strin));
                        }
                    }
                }
            }
        }
    }

    public class DFArabete
    {
        public NodeDFA dest;
        public string path;

        public DFArabete(NodeDFA maqsad, string input)
        {
            dest = maqsad;
            path = input;
        }
    }


    public class NodeDFA
    {
        public List<NodeNFA> node;
        public List<DFArabete> rabete;
        public List<string> harfha;
        public string nam;
        public bool nahaii;
        public bool check;
        public NodeDFA(List<string> horoof)
        {
            nahaii = false;
            node = new List<NodeNFA>();
            harfha = horoof;
            check = false;
            rabete = new List<DFArabete>();
        }
        public void add(NodeNFA nodes)
        {
            node.Add(nodes);
            if (nodes.fstate)
            {
                nahaii = true;
            }
        }

        public bool mojood(NodeNFA node)
        {
            bool javab = false;
            foreach (NodeNFA a in this.node)
            {
                if (a == node)
                {
                    javab = true;
                    break;
                }
            }
            return javab;
        }
        public bool isequal(NodeDFA dovom)
        {
            if (node.Count == dovom.node.Count)
            {
                bool javab = true;
                foreach (NodeNFA a in dovom.node)
                {
                    javab = false;
                    foreach (NodeNFA b in node)
                    {
                        if (a.nam == b.nam)
                        {
                            javab = true;
                            break;
                        }
                    }

                    if (javab==false)
                    {
                        break;
                    }
                }
                return javab;
            }
            else
            {
                return false;
            }
        }
    }


    public class DFA
    {
        public NodeDFA statedakheli;
        public List<NodeDFA> nodes = new List<NodeDFA>();
        public NodeDFA node1;
        public DFA(NodeNFA dakheli, List<string> horoofa)
        {
            statedakheli = new NodeDFA(horoofa);
            statedakheli.node.Add(dakheli);
            nodes = new List<NodeDFA>();
            nodes.Add(statedakheli);
            node1 = new NodeDFA(horoofa);
            NodeNFA t = new NodeNFA("Gt");
            node1.add(t);
            foreach (string a in horoofa)
            {
                node1.rabete.Add(new DFArabete(node1, a));
            }
            node1.check = true;
            nodes.Add(node1);
        }

        public NodeDFA mojooddfa(NodeDFA a)
        {
            NodeDFA javab = null;
            foreach (NodeDFA b in nodes)
            {
                if (b.isequal(a))
                {
                    javab = b;
                    break;
                }
            }
            return javab;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            string inp = Console.ReadLine();
            inp = inp.Trim('{', '}');
            string[] ar = inp.Split(',');
            inp = Console.ReadLine();
            inp = inp.Trim('{', '}');
            string[] alefba = inp.Split(',');
            inp = Console.ReadLine();
            inp = inp.Trim('{', '}');
            string[] fstates = inp.Split(',');
            NFA nfa = new NFA(ar, alefba, fstates);
            List<string> lis = new List<string>();
            nfa.NfAasli = new NFA(lis);
            nfa.NfAasli.Nodedakheli = new NodeNFA("dakhelii");
            nfa.NfAasli.node.Add(nfa.NfAasli.Nodedakheli);
            nfa.NfAasli.node.Add(new NodeNFA("nahai"));
            nfa.NfAasli.node[1].fstate = true;
            nfa.NfAasli.node.Add(new NodeNFA(ar[0]));
            nfa.NfAasli.Nodedakheli.rabete.Add(new NFArabete("", nfa.NfAasli.node[2]));
            nfa.NfAasli.harf = new List<string>(alefba);
            foreach (string s in ar)
            {
                if (s != "q0")
                {
                    NodeNFA s0 = new NodeNFA(s);
                    nfa.NfAasli.node.Add(s0);
                    if (Array.Exists<string>(fstates, x => x == s))
                    {
                        s0.rabete.Add(new NFArabete("", nfa.NfAasli.node[1]));
                    }

                }
            }
            int n = int.Parse(Console.ReadLine());
            for (int i = 0; i < n; i++)
            {
                inp = Console.ReadLine();
                inp = inp.Trim('{', '}');
                nfa.addrabete(inp.Split(','));
            }
            nfa.Nodedakheli = nfa.node[0];

            Console.WriteLine(nfa.convNFAtoDFA());

        }
    }
}
