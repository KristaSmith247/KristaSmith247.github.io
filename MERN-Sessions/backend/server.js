const express = require("express");
const app = express();
const cors = require("cors");

const session = require("express-session");
const MongoStore = require("connect-mongo");

require("dotenv").config({ path: "./config.env" });

const port = process.env.PORT;

app.use(cors(
    {
        // need this for cookies to work
        origin: "http://localhost:3000",
        methods: "GET,HEAD,PUT,PATCH,POST,DELETE",
        credentials: true,
        optionsSuccessStatus: 204,
        allowedHeaders: ["Content-Type", "Authorization"],
    }
));

app.use(session(
    {
        secret: 'keyboard cat',
        saveUninitialized: false, // don't create sessions until something is stored
        resave: false, // don't save session if unmodified
        store: MongoStore.create({
            mongoUrl: process.env.ATLAS_URI
        })
    }
));

const dbo = require("./db/conn");

app.use(express.json());


app.use(require("./routes/record"));
app.use(require("./routes/session"));

app.get("/", (req, res) => {
    res.send("Hello, world!");
});

// const port = 5000; // this is if you want to put the variable directly into the file
// - you usually don't want this

app.listen(port, () => {
    dbo.connectToServer(function (err) {
        if (err) {
            console.err(err);
        }

    });
    console.log(`Server is running on port ${port}`);

});