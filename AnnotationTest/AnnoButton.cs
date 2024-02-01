using ArcGIS.Core.CIM;
using ArcGIS.Core.Data;
using ArcGIS.Core.Data.Mapping;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Catalog;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Editing;
using ArcGIS.Desktop.Editing.Attributes;
using ArcGIS.Desktop.Extensions;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Framework.Dialogs;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Layouts;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnnotationTest
{
    internal class AnnoButton : Button
    {
        protected override void OnClick()
        {
            QueuedTask.Run(() =>
            {
                var selections = MapView.Active.Map.GetSelection();
                var dict = selections.ToDictionary();
                var mapMember = dict.Keys.FirstOrDefault();
                if (mapMember is AnnotationLayer)
                {
                    var oids = dict[mapMember];
                    var qfilter = new QueryFilter() { ObjectIDs = oids };

                    var annoLayer = mapMember as AnnotationLayer;
                    var cursor = annoLayer.Search(qfilter);

                    while (cursor.MoveNext())
                    {
                        var row = cursor.Current;
                        var annoFeature = row as AnnotationFeature;

                        if (annoFeature != null)
                        {
                            bool isShape = false;

                            var insp = new Inspector();
                            insp.Load(annoFeature);

                            var annoProp = insp.GetAnnotationProperties();
                            isShape = annoProp.Shape != null;
                            MessageBox.Show(string.Format("Inspector::GetAnnotationProperties::Shape != null : {0}", isShape));

                            isShape = annoProp.TextGraphic.Shape != null;
                            MessageBox.Show(string.Format("Inspector::GetAnnotationProperties::TextGraphic.Shape != null : {0}", isShape));

                            var annoGraphic = annoFeature.GetGraphic() as CIMTextGraphic;
                            if (annoGraphic != null)
                            {
                                isShape = annoGraphic.Shape != null;
                                MessageBox.Show(string.Format("AnnotationFeature::GetGraphic::Shape != null : {0}", isShape));
                            }

                            
                        }
                    }
                }
            });
        }
    }
}
