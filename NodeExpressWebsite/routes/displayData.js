// display information gathered from user


const express = require("express");
const fs = require('node:fs');
const router = express.Router();

router.get("/", (req, res) => {
    let storedText;
    // second html form variable
    const searchForFood = req.query.searchForFood;
    // try to parse data kept in text file
    try {
        storedText = JSON.parse(fs.readFileSync("answers.txt"))
    } catch (error) {
        // if there is an error, set variable to empty
        storedText = [];
    }

    if (storedText == searchForFood) {
        // if favorite food on file matches entered food,
        // print the name
        res.send("<html><head></head><body>" +
            "<p>People that have this favorite food include: " +
            req.query.firstname +
            " " + req.query.lastname + "</p>" +
            "</body></html>"
        );
    }
});

module.exports = router;
