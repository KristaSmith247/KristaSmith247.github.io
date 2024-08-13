const express = require("express");
const fs = require('node:fs'); // allow files

const router = express.Router();

const answers = []; // create array to hold answers

router.get("/", (req, res) => {
    // create variables to store input
    const firstname = req.query.firstname;
    const lastname = req.query.lastname;
    const food = req.query.food;

    // format input into "content" variable
    const content = "<tr><td>" + firstname + "</td><td>" + lastname + "</td><td>" + food + "</td></tr>";
    answers.push(content); // put content into array

    // store entered information in text file
    fs.appendFile('answers.txt', content, err => {
        if (err) {
            console.error(err);
        }
    });

    res.send("<html><head></head><body><table>" + answers +
        "</table></body>");
});

module.exports = router;
