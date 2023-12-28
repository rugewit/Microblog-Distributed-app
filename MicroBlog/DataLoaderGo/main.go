package main

import (
	"DataLoaderGo/models"
	"bytes"
	"encoding/json"
	"encoding/xml"
	"fmt"
	"io"
	"log"
	"net/http"
	"os"
	"sync"
	"time"
)

func Timer(name string) func() {
	start := time.Now()
	return func() {
		fmt.Printf("%s took %v\n", name, time.Since(start))
		//log.Printf("%s took %v\n", name, time.Since(start))
	}
}

func LoadUserAccounts(path string) ([]models.UserAccount, error) {
	defer Timer("LoadUserAccounts unmarshalling")()
	xmlFilePath := path

	xmlData, err := os.ReadFile(xmlFilePath)
	if err != nil {
		return []models.UserAccount{}, err
	}

	var users models.UserCollection

	err = xml.Unmarshal(xmlData, &users)
	if err != nil {
		return []models.UserAccount{}, err
	}

	return users.Users.UserAccounts, nil
}

func LoadMessages(path string) ([]models.Message, error) {
	defer Timer("LoadMessages unmarshalling")()
	xmlFilePath := path

	xmlData, err := os.ReadFile(xmlFilePath)
	if err != nil {
		return []models.Message{}, err
	}

	var messages models.MessageCollection

	err = xml.Unmarshal(xmlData, &messages)
	if err != nil {
		return []models.Message{}, err
	}

	return messages.Posts.Messages, nil
}

const (
	UsersDatasetPath    = "/home/rugewit/MAI/nosql_dataset/askubuntu/Users.xml"
	MessagesDatasetPath = "/home/rugewit/MAI/nosql_dataset/askubuntu/Posts.xml"
)

type userArray []models.UserAccount
type messageArray []models.Message

func postUsers(users userArray) error {
	usersJson, err := json.Marshal(users)
	if err != nil {
		return nil
	}
	usersBody := bytes.NewBuffer(usersJson)
	resp, err := http.Post("http://localhost:81/api/useraccounts/multiple", "application/json",
		usersBody)
	if err != nil {
		return nil
	}
	body, err := io.ReadAll(resp.Body)
	if err != nil {
		return nil
	}
	fmt.Printf("postUsers resp is:%s\n", string(body))
	return nil
}

func postMessages(messages messageArray) error {
	messagesJson, err := json.Marshal(messages)
	if err != nil {
		return nil
	}
	fmt.Printf("in json: %v\n", string(messagesJson))
	messagesBody := bytes.NewBuffer(messagesJson)
	resp, err := http.Post("http://localhost:81/api/messages/multiple", "application/json",
		messagesBody)
	if err != nil {
		return nil
	}
	body, err := io.ReadAll(resp.Body)
	if err != nil {
		return nil
	}
	fmt.Printf("postMessagesDb resp is:%s\n", string(body))
	return nil
}

func main() {
	defer Timer("main function")()

	wg := sync.WaitGroup{}

	wg.Add(2)

	usersCh := make(chan userArray, 1)
	usersErrCh := make(chan error, 1)

	messageCh := make(chan messageArray, 1)
	messageErrCh := make(chan error, 1)

	// load users
	go func() {
		defer wg.Done()
		users, err := LoadUserAccounts(UsersDatasetPath)
		usersCh <- users
		usersErrCh <- err
	}()

	// load messages
	go func() {
		defer wg.Done()
		messages, err := LoadMessages(MessagesDatasetPath)
		messageCh <- messages
		messageErrCh <- err
	}()

	fmt.Printf("before wait 1\n")
	wg.Wait()
	fmt.Printf("after wait 1\n")

	errFromUsersParsing := <-usersErrCh
	errFromMessagesParsing := <-messageErrCh

	if errFromUsersParsing != nil {
		log.Fatalln(errFromUsersParsing)
	}

	if errFromMessagesParsing != nil {
		log.Fatalln(errFromMessagesParsing)
	}

	fmt.Printf("before reading from channels\n")
	users := <-usersCh
	messages := <-messageCh

	fmt.Printf("before checking\n")
	// check availability
	resp, err := http.Get("http://localhost:81/")
	if err != nil {
		log.Fatalln(err)
	}
	body, err := io.ReadAll(resp.Body)
	if err != nil {
		log.Fatalln(err)
	}
	fmt.Printf("check availability resp is:%s\n", string(body))

	wg.Add(2)

	usersCount := 10
	messagesCount := 10

	go func() {
		defer wg.Done()
		err := postUsers(users[:usersCount])
		if err != nil {
			log.Fatalln(err)
		}
	}()

	go func() {
		defer wg.Done()
		err := postMessages(messages[:messagesCount])
		if err != nil {
			log.Fatalln(err)
		}
	}()

	wg.Wait()
}
