package models

import (
	"encoding/xml"
	//"time"
)

type timeType string

type UserAccount struct {
	ID             string   `bson:"_id,omitempty" json:"id"`
	XmlId          int      `bson:"xml_id" json:"xmlId" xml:"Id,attr"`
	Reputation     int      `bson:"reputation" json:"reputation" xml:"Reputation,attr"`
	CreationDate   timeType `bson:"creation_date" json:"creationDate" xml:"CreationDate,attr"`
	DisplayName    string   `bson:"display_name" json:"displayName" xml:"DisplayName,attr"`
	LastAccessDate timeType `bson:"last_access_date" json:"lastAccessDate" xml:"LastAccessDate,attr"`
	WebsiteUrl     string   `bson:"website_url" json:"websiteUrl" xml:"WebsiteUrl,attr"`
	Location       string   `bson:"location" json:"location" xml:"Location,attr"`
	AboutMe        string   `bson:"about_me" json:"aboutMe" xml:"AboutMe,attr"`
	Views          int      `bson:"views" json:"views" xml:"Views,attr"`
	UpVotes        int      `bson:"up_votes" json:"upVotes" xml:"UpVotes,attr"`
	DownVotes      int      `bson:"down_votes" json:"downVotes" xml:"DownVotes,attr"`
	AccountId      int      `bson:"account_id" json:"accountId" xml:"AccountId,attr"`
}

type UserCollection struct {
	XmlName xml.Name `xml:"UserCollection"`
	Users   Users    `xml:"users"`
}

type Users struct {
	UserAccounts []UserAccount `xml:"row"`
}
