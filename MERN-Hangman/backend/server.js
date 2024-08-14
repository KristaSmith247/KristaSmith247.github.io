const express = require("express");
const app = express();
const cors = require("cors");
require("dotenv").config({ path: "./config.env" });

app.use(cors({
    origin: "http://localhost:3000",
    methods: "GET, HEAD, PUT, PATCH, POST, DELETE",
    credentials: true,
    optionsSuccessStatus: 204,
    allowedHeaders: ["Content-Type", "Authorization"],
}));

app.use(express.json());

const dbo = require("./db/connection"); // retrieves module export process

app.use(require("./routes/gameRoutes"));

const port = process.env.PORT; // go into env file, look for variable called PORT

app.get("/", (req, res) => {
    res.send("Hello, world!");
});

app.listen(port, () => {
    dbo.connectToServer(function (err) {
        if (err) {
            console.err(err);
        }

    });
    console.log(`Server is running on port ${port}`);

});

