package models

import (
	"encoding/xml"
)

type Message struct {
	Id               string   `bson:"_id,omitempty" json:"id"`
	XmlId            int      `bson:"xml_id" json:"xmlId" xml:"Id,attr"`
	PostTypeId       int      `bson:"post_type_id" json:"postTypeId" xml:"PostTypeId,attr"`
	AcceptedAnswerId int      `bson:"accepted_answer_id" json:"acceptedAnswerId" xml:"AcceptedAnswerId,attr"`
	CreationDate     timeType `bson:"creation_date" json:"creationDate" xml:"CreationDate,attr"`
	Score            int      `bson:"score" json:"score" xml:"Score,attr"`
	ViewCount        int      `bson:"view_count" json:"viewCount" xml:"ViewCount,attr"`
	Body             string   `bson:"body" json:"body" xml:"Body,attr"`
	OwnerUserId      int      `bson:"owner_user_id" json:"ownerUserId" xml:"OwnerUserId,attr"`
	LastEditorUserId int      `bson:"last_editor_user_id" json:"lastEditorUserId" xml:"LastEditorUserId,attr"`
	LastEditDate     timeType `bson:"last_edit_date" json:"lastEditDate" xml:"LastEditDate,attr"`
	LastActivityDate timeType `bson:"last_activity_date" json:"lastActivityDate" xml:"LastActivityDate,attr"`
	Title            string   `bson:"title" json:"title" xml:"Title,attr"`
	Tags             string   `bson:"tags" json:"tags" xml:"Tags,attr"`
	AnswerCount      int      `bson:"answer_count" json:"answerCount" xml:"AnswerCount,attr"`
	CommentCount     int      `bson:"comment_count" json:"commentCount" xml:"CommentCount,attr"`
	ContentLicense   string   `bson:"content_license" json:"contentLicense" xml:"ContentLicense,attr"`
}

type MessageCollection struct {
	XmlName xml.Name  `xml:"MessageCollection"`
	Posts   PostArray `xml:"posts"`
}

type PostArray struct {
	Messages []Message `xml:"row"`
}
