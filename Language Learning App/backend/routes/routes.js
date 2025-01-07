const express = require("express");
const crypto = require("crypto"); // used for passwords

// accountRoutes is an instance of the express router.
// We use it to define our routes.
// The router will be added as a middleware and will take control of requests starting with path /record.
const accountRoutes = express.Router();

// This will help us connect to the database
const dbo = require("../db/conn");

// This helps convert the id from string to ObjectId for the _id.
const ObjectId = require("mongodb").ObjectId;

function createAccountId() {
    // create 6-digit account id
    return Math.floor(Math.random() * 1000000).toString().padStart(6, '0');
}


// This section will help you get a list of all the records.
accountRoutes.route("/accounts").get(async (req, res) => {
    try {
        let db_connect = dbo.getDb("languageApp");
        // the following line will return all information about an account except for the password
        const result = await db_connect.collection("accounts")
            .find({})
            .project({ password: 0 })
            .toArray();
        res.json(result);
    } catch (err) {
        throw err;
    }
});

// Login route
accountRoutes.route("/accounts/login").post(async (req, res) => {
    try {
        let db_connect = dbo.getDb();
        let hashedPassword = crypto.createHash('sha256').update(req.body.password).digest('hex');
        let myobj = {
            username: req.body.username,
            password: hashedPassword,
            type: "",
        };
        const checkUserAndPassword = await db_connect
            .collection("accounts")
            .findOne({ username: myobj.username, password: myobj.password });
        if (!checkUserAndPassword) {
            console.log("Error: no user found");
            message = { message: "Error: No user" };
            res.json(message);
        } else {
            message = { message: "Success" };
            res.json(checkUserAndPassword);
        }
    } catch (err) {
        throw err;
    }
});

// This section will help you create a new record.
accountRoutes.route("/accounts/create").post(async (req, res) => {
    try {
        let db_connect = dbo.getDb();
        let hashedPassword = crypto.createHash('sha256').update(req.body.password).digest('hex');
        let accountId = createAccountId();
        let myobj = {
            username: req.body.username,
            password: hashedPassword,
            type: req.body.type,
            accountId: accountId
        };
        const testUsername = await db_connect.collection("accounts").findOne({ username: myobj.username });
        if (testUsername) {
            // if username exists, do not create duplicate account
            res.status(400).json({ message: "Username is already in use" });
            return;
        } else {
            // return success message
            const result = await db_connect.collection("accounts").insertOne(myobj);
            console.log("Connected to collection");
            console.log(result);
            // res.status(201).json({ message: "Account created", account: result.ops[0] });
            res.status(201).json({ message: "Account created successfully" });
        }
    } catch (err) {
        console.log("In catch statement");
        res.status(500).json({ error: err.message });
    }
});

// Fetch single account by id
accountRoutes.route("/accounts/:id").get(async (req, res) => {
    try {
        let db_connect = dbo.getDb();
        let myquery = { _id: new ObjectId(req.params.id) };
        const result = await db_connect.collection("accounts").findOne(myquery);
        res.json(result);
    } catch (err) {
        res.status(500).json({ error: err.message });
    }
});


// Get random work from DB and return
accountRoutes.route("/get-word/general").get(async (req, res) => {
    try {
        let db_connect = dbo.getDb("languageApp");
        const result = await db_connect.collection("words").find({}).toArray();
        const randomWord = result[Math.floor(Math.random() * result.length)]
        res.json(randomWord);
    } catch (err) {
        throw err;
    }
});

// get random word from business collection
accountRoutes.route("/get-word/business").get(async (req, res) => {
    try {
        let db_connect = dbo.getDb("languageApp");
        const result = await db_connect.collection("words").find({ list: "business" }).toArray();
        const randomWord = result[Math.floor(Math.random() * result.length)]
        res.json(randomWord);
    } catch (err) {
        throw err;
    }
});

// get random word from student collection
accountRoutes.route("/get-word/student").get(async (req, res) => {
    try {
        let db_connect = dbo.getDb("languageApp");
        const result = await db_connect.collection("words").find({ list: "student" }).toArray();
        const randomWord = result[Math.floor(Math.random() * result.length)]
        res.json(randomWord);
    } catch (err) {
        throw err;
    }
});

// get random word from travel collection
accountRoutes.route("/get-word/travel").get(async (req, res) => {
    try {
        let db_connect = dbo.getDb("languageApp");
        const result = await db_connect.collection("words").find({ list: "travel" }).toArray();
        const randomWord = result[Math.floor(Math.random() * result.length)]
        res.json(randomWord);
    } catch (err) {
        throw err;
    }
});

// get random word from verb collection
accountRoutes.route("/get-word/verb").get(async (req, res) => {
    try {
        let db_connect = dbo.getDb("languageApp");
        console.log("Found route");
        const result = await db_connect.collection("words").find({ partOfSpeech: "verb" }).toArray();
        const randomWord = result[Math.floor(Math.random() * result.length)]
        res.json(randomWord);
    } catch (err) {
        throw err;
    }
});

accountRoutes.route("/get-word/create").post(async (req, res) => {
    try {
        let db_connect = dbo.getDb();
        let myobj = {
            english: req.body.english,
            spanish: req.body.spanish,
            partOfSpeech: req.body.partOfSpeech,
            tense: req.body.tense,
            list: req.body.list,
        };
        const findWord = await db_connect.collection("words").findOne({ english: myobj.english });
        if (findWord) {
            // if username exists, do not create duplicate account
            res.status(400).json({ message: "Word is already in database" });
        } else {
            // return success message
            const result = db_connect.collection("words").insertOne(myobj);
            console.log("Added a word");
            res.status(201).json({ message: "Word added", word: result.ops[0] });
            //res.status(201).json(result);
        }
    } catch (err) {
        res.status(500).json({ error: err.message });
    }
});

// Post

module.exports = accountRoutes;
