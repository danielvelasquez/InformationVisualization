using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class Link
    {
        public Node source;
        public Node target;
        public string type;
        
        /*public Link(Node source, Node target, string type) {
            this.source = source;
            this.target = target;
            this.type = type;
        }*/
    }
}