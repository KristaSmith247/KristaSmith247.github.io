const express = require("express");
const app = express();
const cors = require("cors");
require("dotenv").config({ path: "./config.env" });

app.use(cors());
app.use(express.json());
app.use(require("./routes/record")); // ADDED THIS

const dbo = require("./db/conn"); // retrieves module export process

const port = process.env.PORT; // go into env file, look for variable called PORT

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
