namespace PlexCopier
{
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "ffprobe", Namespace = "", IsNullable = false)]
    public partial class ffprobe
    {

        private ffprobeChapter[] chaptersField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("chapter", IsNullable = false)]
        public ffprobeChapter[] chapters
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
