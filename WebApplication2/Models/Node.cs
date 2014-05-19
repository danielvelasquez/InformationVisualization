using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class Node
    {
        public string type;
        public string name;
        public int? id;
        public List<Node> children;
        public List<Node> _children;
        public Node(string type, string name, int? id) {
            this.type = type;
            this.name = name;
            this.id = id;
            children = new List<Node>();
            _children = new List<Node>();
        }
        public bool equals(Object o) { 
            if(o==null || o.GetType() != typeof(Node)){
                return false;
            }else{
                if (((Node)o).id != this.id) { return false; }
            }
            return true;
        }
    }
}