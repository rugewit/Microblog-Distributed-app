#!/bin/bash

DATABASE_PATH="/home/rugewit/Programming/Microblog-Distributed-app/MicroBlog/MicroBlog/Database"
mongod --dbpath $DATABASE_PATH
use MicroBlog