using System;
using System.Xml;
using System.Text;
using System.Collections.Generic;

/*
 * Copyright 2010 Google Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not
 * use this file except in compliance with the License. You may obtain a copy of
 * the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
 * License for the specific language governing permissions and limitations under
 * the License.
 */
namespace Feng.Map
{
    using Location = GMap.NET.PointLatLng;
    using Track = GMap.NET.gpxType;

    using SAXException = System.Xml.XmlException;
    using NumberFormatException = InvalidCastException;

    ///
    /// <summary> * Imports GPX XML files to the my tracks provider.
    /// *
    /// * TODO: Show progress indication to the user.
    /// *
    /// * @author Leif Hendrik Wilden
    /// * @author Steffen Horlacher
    /// * @author Rodrigo Damazio </summary>
    /// 
    [CLSCompliant(false)]
    public class GpxImporter// : org.xml.sax.helpers.DefaultHandler
    {

        /*
         * GPX-XML tag names and attributes.
         */
        private const string TAG_TRACK = "trk";
        private const string TAG_TRACK_POINT = "trkpt";
        private static readonly object TAG_TRACK_SEGMENT = "trkseg";
        private const string TAG_NAME = "name";
        private const string TAG_DESCRIPTION = "desc";
        private const string TAG_ALTITUDE = "ele";
        private const string TAG_TIME = "time";
        private const string ATT_LAT = "lat";
        private const string ATT_LON = "lon";

        ///  
        ///   <summary> * The maximum number of locations to buffer for bulk-insertion into the database. </summary>
        ///   
        private const int MAX_BUFFERED_LOCATIONS = 512;

        ///  
        ///   <summary> * List of track ids written in the database. Only contains successfully
        ///   * written tracks. </summary>
        ///   
        private readonly IList<Track> tracksWritten;

        ///  
        ///   <summary> * Contains the current elements content. </summary>
        ///   
        private string content;

        ///  
        ///   <summary> * Currently reading location. </summary>
        ///   
        private Location location;

        ///  
        ///   <summary> * Previous location, required for calculations. </summary>
        ///   
        private Location lastLocation;

        ///  
        ///   <summary> * Currently reading track. </summary>
        ///   
        private Track track;
        private List<GMap.NET.trkType> trkTypes = new List<GMap.NET.trkType>();
        private List<GMap.NET.wptType> wptTypes = new List<GMap.NET.wptType>();

        ///  
        ///   <summary> * Buffer of locations to be bulk-inserted into the database. </summary>
        ///   
        private Location[] bufferedPointInserts = new Location[MAX_BUFFERED_LOCATIONS];

        ///  
        ///   <summary> * Number of locations buffered to be inserted into the database. </summary>
        ///   
        private int numBufferedPointInserts = 0;

        ///  
        ///   <summary> * Number of locations already processed. </summary>
        ///   
        private int numberOfLocations;

        ///  
        ///   <summary> * Number of segments already processed. </summary>
        ///   
        private int numberOfSegments;

        ///  
        ///   <summary> * Used to identify if a track was written to the database but not yet
        ///   * finished successfully. </summary>
        ///   
        private bool isCurrentTrackRollbackable;

        ///  
        ///   <summary> * Flag to indicate if we're inside a track's xml element.
        ///   * Some sub elements like name may be used in other parts of the gpx file,
        ///   * and we use this to ignore them. </summary>
        ///   
        private bool isInTrackElement;

        ///  
        ///   <summary> * Counter to find out which child level of track we are processing. </summary>
        ///   
        private int trackChildDepth;

        ///  
        ///   <summary> * SAX-Locator to get current line information. </summary>
        ///   
        //private Locator locator;
        private Location lastSegmentLocation;

        private void ParseURL(string strUrl)
        {
            strUrl = strUrl.Replace("\\n", "\r\n");
 
            try
            {
                XmlTextReader reader = new XmlTextReader(new System.IO.StringReader(strUrl));
                while (reader.Read())
                {
                    try
                    {
                        switch (reader.NodeType)
                        {
                            case XmlNodeType.Element:
                                {
                                    var attributes = new Dictionary<string, string>();
                                    string strURI = reader.NamespaceURI;
                                    string strName = reader.Name;
                                    if (reader.HasAttributes)
                                    {
                                        for (int i = 0; i < reader.AttributeCount; i++)
                                        {
                                            reader.MoveToAttribute(i);
                                            attributes.Add(reader.Name, reader.Value);
                                        }
                                    }
                                    startElement(strURI, strName, strName, attributes);
                                }
                                break;
                            case XmlNodeType.EndElement:
                                {
                                    string strURI = reader.NamespaceURI;
                                    string strName = reader.Name;
                                    endElement(strURI, strName, strName);
                                }
                                break;
                            case XmlNodeType.CDATA:
                                {
                                    content = reader.Value;
                                }
                                break;
                            case XmlNodeType.Text:
                                {
                                    content = reader.Value;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            catch (XmlException e)
            {
                Console.WriteLine("error occured: " + e.Message);
            }
        }
        ///  
        ///   <summary> * Reads GPS tracks from a GPX file and writes tracks and their coordinates to
        ///   * the database.
        ///   *  </summary>
        ///   * <param name="is"> a input steam with gpx-xml data </param>
        ///   * <returns> long[] array of track ids written in the database </returns>
        ///   * <exception cref="SAXException"> a parsing error </exception>
        ///   * <exception cref="ParserConfigurationException"> internal error </exception>
        ///   * <exception cref="IOException"> a file reading problem </exception>
        ///   
        public static GMap.NET.gpxType[] importGPXFile(string content)
        {
            //SAXParserFactory factory = SAXParserFactory.newInstance();
            GpxImporter handler = new GpxImporter();
            //SAXParser parser = factory.newSAXParser();
            Track[] trackIds = null;

            try
            {
                //long start = Java.Lang.JavaSystem.CurrentTimeMillis();

                handler.ParseURL(content);
                //parser.parse(@is, handler);

                //long end = Java.Lang.JavaSystem.CurrentTimeMillis();
                //Log.Debug(Constants.TAG, "Total import time: " + (end - start) + "ms");

                trackIds = handler.ImportedTrackIds;
            }
            finally
            {
                // delete track if not finished
                handler.rollbackUnfinishedTracks();
            }

            return trackIds;
        }

        ///  
        ///   <summary> * Constructor, requires providerUtils for writing tracks the database. </summary>
        ///   
        public GpxImporter()
        {
            tracksWritten = new List<Track>();
        }

        //public override void characters(char[] ch, int start, int length)
        //{
        //  string newContent = new string(ch, start, length);
        //  if(content == null)
        //  {
        //    content = newContent;
        //  }
        //  else
        //  {
        //    // In 99% of the cases, a single call to this method will be made for each
        //    // sequence of characters we're interested in, so we'll rarely be
        //    // concatenating strings, thus not justifying the use of a StringBuilder.
        //    content += newContent;
        //  }
        //}

        public void startElement(string uri, string localName, string name, IDictionary<string, string> attributes)
        {
            if (isInTrackElement)
            {
                trackChildDepth++;
                if (localName.Equals(TAG_TRACK_POINT))
                {
                    onTrackPointElementStart(attributes);
                }
                else if (localName.Equals(TAG_TRACK_SEGMENT))
                {
                    onTrackSegmentElementStart();
                }
                else if (localName.Equals(TAG_TRACK))
                {
                    string msg = createErrorMessage("Invalid GPX-XML detected");
                    throw new SAXException(msg);
                }
            }
            else if (localName.Equals(TAG_TRACK))
            {
                isInTrackElement = true;
                trackChildDepth = 0;
                onTrackElementStart();
            }
        }

        public void endElement(string uri, string localName, string name)
        {
            if (!isInTrackElement)
            {
                content = null;
                return;
            }

            // process these elements only as sub-elements of track
            if (localName.Equals(TAG_TRACK_POINT))
            {
                onTrackPointElementEnd();
            }
            else if (localName.Equals(TAG_ALTITUDE))
            {
                onAltitudeElementEnd();
            }
            else if (localName.Equals(TAG_TIME))
            {
                onTimeElementEnd();
            }
            else if (localName.Equals(TAG_NAME))
            {
                // we are only interested in the first level name element
                if (trackChildDepth == 1)
                {
                    onNameElementEnd();
                }
            }
            else if (localName.Equals(TAG_DESCRIPTION))
            {
                // we are only interested in the first level description element
                if (trackChildDepth == 1)
                {
                    onDescriptionElementEnd();
                }
            }
            else if (localName.Equals(TAG_TRACK_SEGMENT))
            {
                onTrackSegmentElementEnd();
            }
            else if (localName.Equals(TAG_TRACK))
            {
                onTrackElementEnd();
                isInTrackElement = false;
                trackChildDepth = 0;
            }
            trackChildDepth--;

            // reset element content
            content = null;
        }

        //public override Locator DocumentLocator
        //{
        //    set
        //    {
        //      this.locator = value;
        //    }
        //}

        ///  
        ///   <summary> * Create a new Track object and insert empty track in database. Track will be
        ///   * updated with missing values later. </summary>
        ///   
        private void onTrackElementStart()
        {
            track = new Track();
            trkTypes.Clear();
            wptTypes.Clear();

            numberOfLocations = 0;

            //Uri trackUri = providerUtils.InsertTrack(track);
            //long trackId = Convert.ToInt64(trackUri.LastPathSegment);
            //track.Id = trackId;
            isCurrentTrackRollbackable = true;
        }

        private void onDescriptionElementEnd()
        {
            //track.Description = content.ToString().Trim();
        }

        private void onNameElementEnd()
        {
            track.creator = content.ToString().Trim();
        }

        ///  
        ///   <summary> * Track segment started. </summary>
        ///   
        private void onTrackSegmentElementStart()
        {
            if (numberOfSegments > 0)
            {
                // Add a segment separator:
                location = new Location();
                location.Lat = 0.0;
                location.Lng = 0.0;
                //location.Altitude = 0;
                //if (lastLocation != Location.Zero)
                //{
                //    location.Time = lastLocation.Time;
                //}
                insertTrackPoint(location);
                lastLocation = location;
                //lastSegmentLocation = null;
                location = Location.Zero;
            }

            numberOfSegments++;
        }

        ///  
        ///   <summary> * Reads trackpoint attributes and assigns them to the current location.
        ///   * </summary>
        ///   * <param name="attributes"> xml attributes </param>
        ///   
        private void onTrackPointElementStart(IDictionary<string, string> attributes)
        {
            if (location != Location.Zero)
            {
                string errorMsg = createErrorMessage("Found a track point inside another one.");
                throw new SAXException(errorMsg);
            }

            location = createLocationFromAttributes(attributes);
        }

        ///  
        ///   <summary> * Creates and returns a location with the position parsed from the given
        ///   * attributes.
        ///   * </summary>
        ///   * <param name="attributes"> the attributes to parse </param>
        ///   * <returns> the created location </returns>
        ///   * <exception cref="SAXException"> if the attributes cannot be parsed </exception>
        ///   
        private Location createLocationFromAttributes(IDictionary<string, string> attributes)
        {
            string latitude = attributes[ATT_LAT];
            string longitude = attributes[ATT_LON];

            if (latitude == null || longitude == null)
            {
                throw new SAXException(createErrorMessage("Point with no longitude or latitude"));
            }

            // create new location and set attributes
            Location loc = new Location();
            try
            {
                loc.Lat = Convert.ToDouble(latitude);
                loc.Lng = Convert.ToDouble(longitude);
            }
            catch (NumberFormatException e)
            {
                string msg = createErrorMessage("Unable to parse lat/long: " + latitude + "/" + longitude);
                throw new SAXException(msg, e);
            }
            return loc;
        }

        ///  
        ///   <summary> * Track point finished, write in database.
        ///   *  </summary>
        ///   * <exception cref="SAXException"> - thrown if track point is invalid </exception>
        ///   
        private void onTrackPointElementEnd()
        {
            if (true)
            {
                // insert in db
                insertTrackPoint(location);

                // first track point?
                if (lastLocation == null && numberOfSegments == 1)
                {
                    //track.StartId = LastPointId;
                }

                lastLocation = location;
                lastSegmentLocation = location;
                location = Location.Zero;
            }
            //else
            //{
            //    // invalid location - abort import
            //    string msg = createErrorMessage("Invalid location detected: " + location);
            //    throw new SAXException(msg);
            //}
        }

        private void insertTrackPoint(Location loc)
        {
            bufferedPointInserts[numBufferedPointInserts] = loc;
            numBufferedPointInserts++;
            numberOfLocations++;

            if (numBufferedPointInserts >= MAX_BUFFERED_LOCATIONS)
            {
                flushPointInserts();
            }
        }

        private void flushPointInserts()
        {
            if (numBufferedPointInserts <= 0)
            {
                return;
            }

            GMap.NET.trkType trkType = new GMap.NET.trkType();
            trkType.trkseg = new GMap.NET.trksegType[1];
            List<GMap.NET.wptType> list = new List<GMap.NET.wptType>();
            for(int i=0; i<numBufferedPointInserts; ++i)
            {
                var j = new GMap.NET.wptType();
                j.lat = (decimal)bufferedPointInserts[i].Lat;
                j.lon = (decimal)bufferedPointInserts[i].Lng;
                list.Add(j);
            }
            trkType.trkseg[0].trkpt = list.ToArray();

            trkTypes.Add(trkType);

            numBufferedPointInserts = 0;
        }

        ///  
        ///   <summary> * Track segment finished. </summary>
        ///   
        private void onTrackSegmentElementEnd()
        {
            // Nothing to be done
        }

        ///  
        ///   <summary> * Track finished - update in database. </summary>
        ///   
        private void onTrackElementEnd()
        {
            if (lastLocation != Location.Zero)
            {
                flushPointInserts();

                //track.StopId = LastPointId;
                track.trk = trkTypes.ToArray();
                track.wpt = wptTypes.ToArray();

                tracksWritten.Add(track);
                isCurrentTrackRollbackable = false;

                isCurrentTrackRollbackable = false;
                lastSegmentLocation = Location.Zero;
                lastLocation = Location.Zero;
            }
            else
            {
                // track contains no track points makes no real
                // sense to import it as we have no location
                // information -> roll back
                rollbackUnfinishedTracks();
            }
        }

        ///  
        ///   <summary> * Setting time and doing additional calculations as this is the last value
        ///   * required. Also sets the start time for track and statistics as there is no
        ///   * start time in the track root element.
        ///   *  </summary>
        ///   * <exception cref="SAXException"> on parsing errors </exception>
        ///   
        private void onTimeElementEnd()
        {
            if (location == null)
            {
                return;
            }

            // Parse the time
            DateTime datetime;
            try
            {
                datetime = DateTime.Parse(content);// StringUtils.parseXmlDateTime(content.Trim());
            }
            catch (System.Exception e)
            {
                string msg = createErrorMessage("Unable to parse time: " + content);
                throw new SAXException(msg, e);
            }
            //long time = MonoDroidLib.Help4Net.DateTimeToUtcLong(datetime);

            // Calculate derived attributes from previous point
            if (lastSegmentLocation != Location.Zero)
            {
                long timeDifference = 0;// time - lastSegmentLocation.Time;

                // check for negative time change
                if (timeDifference < 0)
                {
                }
                else
                {

                    // We don't have a speed and bearing in GPX, make something up from
                    // the last two points.
                    // TODO GPS points tend to have some inherent imprecision,
                    // speed and bearing will likely be off, so the statistics for things like
                    // max speed will also be off.
                    //float speed = location.DistanceTo(lastLocation) * 1000.0f / timeDifference;
                    //location.Speed = speed;
                    //location.Bearing = lastSegmentLocation.BearingTo(location);
                }
            }

            //// Fill in the time
            //location.Time = time;
        }

        private void onAltitudeElementEnd()
        {
            if (location != Location.Zero)
            {
                try
                {
                    //location.Altitude = Convert.ToDouble(content);
                }
                catch (NumberFormatException e)
                {
                    string msg = createErrorMessage("Unable to parse altitude: " + content);
                    throw new SAXException(msg, e);
                }
            }
        }

        ///  
        ///   <summary> * Deletes the last track if it was not completely imported. </summary>
        ///   
        public virtual void rollbackUnfinishedTracks()
        {
            if (isCurrentTrackRollbackable)
            {
                isCurrentTrackRollbackable = false;
            }
        }

        ///  
        ///   <summary> * Get all track ids of the tracks created by this importer run.
        ///   *  </summary>
        ///   * <returns> array of track ids </returns>
        ///   
        private Track[] ImportedTrackIds
        {
            get
            {
                // Convert from java.lang.Long for convenience
                Track[] result = new Track[tracksWritten.Count];
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = tracksWritten[i];
                }
                return result;
            }
        }

        ///  
        ///   <summary> * Returns the ID of the last point inserted into the database. </summary>
        ///   
        private long LastPointId
        {
            get
            {
                flushPointInserts();
                return 0;
            }
        }

        ///  
        ///   <summary> * Builds a parsing error message with current line information.
        ///   *  </summary>
        ///   * <param name="details"> details about the error, will be appended </param>
        ///   * <returns> error message string with current line information </returns>
        ///   
        private string createErrorMessage(string details)
        {
            StringBuilder msg = new StringBuilder();
            msg.Append("Parsing error at line: ");
            //msg.Append(locator.LineNumber);
            msg.Append(" column: ");
            //msg.Append(locator.ColumnNumber);
            msg.Append(". ");
            msg.Append(details);
            return msg.ToString();
        }
    }

}