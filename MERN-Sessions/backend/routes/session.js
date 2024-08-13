const express = require("express");
const recordRoutes = require("./record");
const routes = express.Router();

routes.route("/session_set").get(async function (req, res) {
    console.log("In /session_set, session is: " + req.session);
    let status = "";
    if (!req.session.username) {
        // create new session if no username
        req.session.username = "john";
        status = "Session set";
        console.log(status);
    } else {
        status = "Session already existed";
        console.log(status);
    }
    const resultObj = { status: status }; // key is called status, variable is second

    res.json(resultObj);
});


routes.route("/session_get").get(async function (req, res) {
    console.log("In /session_get, session is: " + req.session);
    let status = "";
    if (!req.session.username) {
        status = "No session set";
        console.log(status);
    } else {
        status = "Session username is: " + req.session.username;
        console.log(status);
    }
    const resultObj = { status: status }; // key is called status, variable is second

    res.json(resultObj);
});


routes.route("/session_delete").get(async function (req, res) {
    console.log("In /session_delete, session is: " + req.session);
    req.session.destroy();
    let status = "No session set";

    const resultObj = { status: status }; // key is called status, variable is second

    res.json(resultObj);
});

module.exports = routes;
