// file to keep records of accounts


const express = require("express");

// recordRoutes is an instance of the express router.
// We use it to define our routes.
// The router will be added as a middleware and will take control of requests starting with path /record.
const recordRoutes = express.Router();

// This will help us connect to the database
const dbo = require("../db/conn");

// This helps convert the id from string to ObjectId for the _id.
const ObjectId = require("mongodb").ObjectId;


// This section will help you get a list of all the records.
recordRoutes.route("/record").get(async (req, res) => {
    try {
        let db_connect = dbo.getDb("checkingAccounts");
        // the following line will return all information about an account except for the password
        const result = await db_connect.collection("records").find({}).project({ password: 0 }).toArray();
        res.json(result);
    } catch (err) {
        throw err;
    }
});

// This section will help you get a single record by id
recordRoutes.route("/record/:id").get(async (req, res) => {
    try {
        let db_connect = dbo.getDb();
        let myquery = { _id: new ObjectId(req.params.id) };
        const result = await db_connect.collection("records").findOne(myquery);
        res.json(result);
    } catch (err) {
        throw err;
    }
});

// TEST: get a record associated with an email
recordRoutes.route("/record/:emailAddress").post(async (req, res) => {
    try {
        let db_connect = dbo.getDb("checkingAccounts");
        // works with hardcoded email value
        const result = await db_connect.collection("records")
            .find({ emailAddress: "marnie@email.com" })
            .project({ password: 0 }).toArray();

        // let user enter email to search for
        // let myEmailSearch = { _id: req.params.emailAddress };
        // const result = await db_connect.collection("records")
        //     .findOne(myEmailSearch)
        //     .project({ password: 0 }).toArray();
        res.json(result);
    } catch (err) {
        throw err;
    }
});

// This section will help you create a new record.
recordRoutes.route("/record/add").post(async (req, res) => {
    try {
        let db_connect = dbo.getDb();
        // verify if email already in database
        // EDIT : look up email, then check if password matches
        let testEmail = await db_connect.collection("records").findOne({ emailAddress: req.body.emailAddress });
        let testPassword = await db_connect.collection("records").findOne({ password: req.body.password });

        if (testEmail && testPassword) {
            return res.status(409).send({ message: "Email and password already in use" })
        }
        else {
            let myobj = {
                firstName: req.body.firstName,
                lastName: req.body.lastName,
                emailAddress: req.body.emailAddress,
                phone: req.body.phone,
                password: req.body.password,
                role: "", // role is blank for now
                checking: 0, // account starts at 0
                savings: 0,
                accountType: req.body.accountType // tells if checking or savings account
            };

            const result = db_connect.collection("records").insertOne(myobj);
            console.log("Created an account");
            res.json(result);
        }
    } catch (err) {
        throw err;
    }
});


// This section will help you update a record by id.
recordRoutes.route("/update/:id").put(async (req, res) => {
    try {
        let db_connect = dbo.getDb();
        let myquery = { _id: new ObjectId(req.params.id) };
        let newvalues = {
            $set: {
                firstName: req.body.firstName,
                lastName: req.body.lastName,
                emailAddress: req.body.emailAddress,
                phone: req.body.phone,
                password: req.body.password,
                role: req.body.role, // role is being updated
            },
        };
        const result = db_connect.collection("records").updateOne(myquery, newvalues);
        console.log("1 account updated");
        res.json(result);
    } catch (err) {
        throw err;
    }
});


// update a record by email address
recordRoutes.route("/update/:emailAddress").post(async (req, res) => {
    try {
        let db_connect = dbo.getDb();
        let emailToUpdate = { emailAddress: "marnie@email.com" };
        //let myquery = { _id: new ObjectId(req.params.id) };
        let newvalues = {
            $set: {
                firstName: req.body.firstName,
                lastName: req.body.lastName,
                emailAddress: req.body.emailAddress,
                phone: req.body.phone,
                password: req.body.password,
                role: req.body.role, // role is being updated
            },
        };
        if (req.body.role !== "Manager" || "manager" || "Customer" || "customer" || "Administrator" || "administrator") {
            return res.status(409).send({ message: "Allowed roles are manager, customer, and administrator" });
        };
        const result = db_connect.collection("records").updateOne(emailToUpdate, newvalues);
        console.log("1 document updated");
        res.json(result);
    } catch (err) {
        throw err;
    }
});

// This section will help you delete a record
recordRoutes.route("/:id").delete(async (req, res) => {
    try {
        let db_connect = dbo.getDb();
        let myquery = { _id: new ObjectId(req.params.id) };
        const result = db_connect.collection("records").deleteOne(myquery);
        console.log("1 document deleted");
        res.json(result);
    } catch (err) {
        throw err;
    }
});


recordRoutes.route("/record/deposit/:depositAmount").post(async (req, res) => {
    try {
        
        // get account to deposit in
        let db_connect = dbo.getDb();
        let myquery = { emailAddress: "marnie@email.com" };
        let accountType = req.body.accountType;

        // get current amount from specified account
        let currentBalance = myquery[accountType];
        if (accountType === "checking") {
            // add to checking account
            let newvalues = {
                $set: {
                    checking: parseInt(currentBalance) + parseInt(req.body.depositAmount)
                }
            };
        }
        // add to savings account
        if (accountType === "savings") {
            let newvalues = {
                $set: {
                    savings: parseInt(currentBalance) + parseInt(req.body.depositAmount)
                }
            };
        }

        // update amount
        const result = db_connect.collection("records").updateOne(myquery);
        console.log("Deposit successful");
        res.json(result);
    }
    catch (err) { throw err; }
});



recordRoutes.route("/record/withdrawal/:withdrawalAmount").post(async (req, res) => {
    try {
        // get account to withdrawal from
        let db_connect = dbo.getDb();
        let myquery = { emailAddress: "marnie@email.com" };
        let accountType = req.body.accountType;

        // get current amount from specified account
        let currentBalance = myquery[accountType];
        // check to see if amount available to withdrwal
        if (withdrawalAmount <= currentBalance) {
            if (accountType === "checking") {
                // withdrawal from checking account
                let newvalues = {
                    $set: {
                        checking: parseInt(currentBalance) - parseInt(req.body.withdrawalAmount)
                    }
                };
            }
            // withdrawal from savings account
            if (accountType === "savings") {
                let newvalues = {
                    $set: {
                        savings: parseInt(currentBalance) - parseInt(req.body.withdrawalAmount)
                    }
                };
            }
            // update amount
            const result = db_connect.collection("records").updateOne(myquery);
            console.log("Withdrawal successful");
            res.json(result);
        }
        else {
            return res.status(409).send({ message: "Withdrawal amount cannot be greater than account balance" });
        }


    }
    catch (err) { throw err; }
});


recordRoutes.route("/record/:emailAddress/transfer").post(async (req, res) => {
    // transfer funds in an account from checking to savings
    try {
        // find email to account to update
        let db_connect = dbo.getDb();
        let myquery = { emailAddress: "marnie@email.com" };
        let accountType = req.body.accountType;

        // withdrawal amount from given account type (if withdrawal possible)
        if (withdrawalAmount <= currentBalance) {
            if (accountType === "checking") {
                // withdrawal from checking account, deposit to savings
                let newvalues = {
                    $set: {
                        checking: parseInt(currentBalance) - parseInt(req.body.depositAmount),
                        savings: parseInt(currentBalance) + parseInt(req.body.depositAmount)
                    }
                };

            }
            // withdrawal from savings account, transfer to checking
            if (accountType === "savings") {
                let newvalues = {
                    $set: {
                        savings: parseInt(currentBalance) - parseInt(req.body.depositAmount),
                        checking: parseInt(currentBalance) + parseInt(req.body.depositAmount)
                    }
                };
            }
            // update amount
            const result = db_connect.collection("records").updateOne(myquery);
            console.log("Transfer successful");
            res.json(result);
        }
        else {
            return res.status(409).send({ message: "Transfer amount cannot be greater than account balance" });
        }

    }
    catch (err) {
        throw err;
    }
});























// recordRoutes.route("record/:emailAddress").post(async (req, res) => {

//     // find records

//     // Execute query
//     let db_connect = dbo.getDb();
//     const records = db_connect.collection("records");  // Search the records collection within the db
//     const query = { emailAddress: "otherEmail@email.com", password: "thisPassword" };
//     const options = {};
//     const cursor = records.find(query, options);
//     const numResults = await records.countDocuments(query);

//     // Print a message if no documents were found
//     if (numResults === 0) {
//         console.log("No account found!");
//     } else {
//         console.log(`Found ${numResults} account`);
//     }

//     // Print returned documents
//     for await (const doc of cursor) {
//         console.dir(doc);
//     }
// });



module.exports = recordRoutes;
