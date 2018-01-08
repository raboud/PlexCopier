namespace MediaLib
{
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "ffprobe", Namespace = "", IsNullable = false)]
    public partial class Chapters
    {

        private Chapter[] chaptersField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("chapter", IsNullable = false)]
        public Chapter[] chapters
        {
            get
            {
                return this.chaptersField;
            }
            set
            {
                this.chaptersField = value;
            }
        }
    }


}
