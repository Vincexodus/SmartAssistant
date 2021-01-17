from googleapiclient.discovery import build

uploadId = ""
channelID = ""

api_key = "AIzaSyBgvuTnJBhDRQBoL02se5GGLKp2jYJwu04"

youtube = build("youtube", "v3", developerKey=api_key)


# To extract channel id
channel_id_request = youtube.channels().list(
    part = "snippet, contentDetails, id",
    forUsername = "pewdiepie"
)

channel_id_response = channel_id_request.execute()
# print(channel_id_response)

for items in channel_id_response["items"]:
     channelID = items["id"]



#To search and obtain video id
channel_search_request = youtube.search().list(
    order = "date",
    part = "snippet",
    channelId = channelID
)

vids = []

channel_search_response = channel_search_request.execute()

#Group the videos id into list
for items in channel_search_response["items"]:
    vids.append(items["id"]["videoId"])


#To obtain video info from video id
video_request = youtube.videos().list(
    part =" contentDetails",
    id = ",".join(vids)
)

video_response = video_request.execute()

print(video_response)
'''
for items in video_response["items"]:
    title = items["snippet"]["title"]
    print(f"Title: {title}")
    views = items["statistics"]["viewCount"]
    print(f"Views: {views}")

# print(channel_search_response)'''