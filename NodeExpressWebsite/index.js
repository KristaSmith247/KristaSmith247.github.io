const express = require("express");
const myCustomRoutes = require("./routes/user");

// load express
const app = express();
const port = 3000;

// use routes in /routes/user
app.use("/user_routes", myCustomRoutes);

// demonstrate a route in index.js
app.get("/", (req, res) => {
    res.send("Node and Express Project");
});

// normally we wouldn't do this, React will be a frontend
// allow user to see html files
app.use(express.static("public"));

// set up server
app.listen(port, () => {
    console.log("Server started on port " + port);
});
