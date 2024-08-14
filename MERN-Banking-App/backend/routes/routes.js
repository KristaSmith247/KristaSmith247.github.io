const express = require("express");
const crypto = require("crypto");

const accountRoutes = express.Router();

const dbo = require("../db/connection");

const ObjectId = require("mongodb").ObjectId;

function generateCustomerId() {
    return 'C' + Math.floor(Math.random() * 1000000).toString().padStart(6, '0');
}

accountRoutes.route("/accounts/add").post(async (req, res) => {
    try {
        let db_connect = dbo.getDb();
        let hashedPassword = crypto.createHash('sha256').update(req.body.password).digest('hex');
        let customerId = generateCustomerId();
        let myobj = {
            firstname: req.body.firstname,
            lastname: req.body.lastname,
            email: req.body.email,
            phone: req.body.phone,
            password: hashedPassword,
            savings: 0,
            checking: 0,
            investment: 0,
            role: req.body.role,
            customerId: customerId,
        };
        const checkEmail = await db_connect.collection("users").findOne({ email: myobj.email });
        if (checkEmail) {
            res.status(400).json({ message: "Error: Email already used." });
        } else {
            const result = await db_connect.collection("users").insertOne(myobj);
            res.status(201).json({ message: "Success", user: result.ops[0] });
        }
    } catch (err) {
        res.status(500).json({ error: err.message });
    }
});

// Deposit Route
accountRoutes.route("/accounts/deposit").post(async (req, res) => {
    console.log("Deposit route hit");
    try {
        let db_connect = dbo.getDb();
        const { customerId, amount, accountType } = req.body;
        console.log("Deposit request body:", req.body);

        const user = await db_connect.collection("users").findOne({ customerId });
        console.log("Found user:", user); // Log the found user or null

        if (user) {
            let newBalance = user[accountType] + parseFloat(amount);
            await db_connect.collection("users").updateOne({ customerId }, { $set: { [accountType]: newBalance } });

            const transaction = {
                customerId: customerId,
                type: "deposit",
                amount: parseFloat(amount),
                account: accountType,
                date: new Date(),
                description: `Deposited ${amount} to ${accountType}`,
            };
            await db_connect.collection("transactions").insertOne(transaction);
            res.json({ message: "Deposit successful", newBalance });
        } else {
            console.log("User not found:", customerId); // Debug log
            res.status(404).json({ message: "User not found" });
        }
    } catch (err) {
        console.error("Deposit route error:", err); // Debug log
        res.status(500).json({ error: err.message });
    }
});

// Withdraw Route
accountRoutes.route("/accounts/withdraw").post(async (req, res) => {
    console.log("Withdraw route hit");
    try {
        let db_connect = dbo.getDb();
        const { customerId, amount, accountType } = req.body;
        console.log("Withdraw request body:", req.body);

        const user = await db_connect.collection("users").findOne({ customerId });
        console.log("Found user:", user); // Log the found user or null

        if (user) {
            let newBalance = user[accountType] - parseFloat(amount);
            if (newBalance < 0) {
                res.status(400).json({ message: "Insufficient funds" });
                return;
            }
            await db_connect.collection("users").updateOne({ customerId }, { $set: { [accountType]: newBalance } });

            const transaction = {
                customerId: customerId,
                type: "withdraw",
                amount: parseFloat(amount),
                account: accountType,
                date: new Date(),
                description: `Withdrew ${amount} from ${accountType}`,
            };
            await db_connect.collection("transactions").insertOne(transaction);
            res.json({ message: "Withdrawal successful", newBalance });
        } else {
            console.log("User not found:", customerId); // Debug log
            res.status(404).json({ message: "User not found" });
        }
    } catch (err) {
        console.error("Withdraw route error:", err); // Debug log
        res.status(500).json({ error: err.message });
    }
});

// Transfer Route
accountRoutes.route("/accounts/transfer").post(async (req, res) => {
    console.log("Transfer route hit");
    try {
        let db_connect = dbo.getDb();
        const { customerId, fromAccountType, toAccountType, amount } = req.body;
        console.log("Transfer request body:", req.body);

        const user = await db_connect.collection("users").findOne({ customerId });
        console.log("Found user:", user); // Log the found user or null

        if (user) {
            let newFromBalance = user[fromAccountType] - parseFloat(amount);
            if (newFromBalance < 0) {
                res.status(400).json({ message: "Insufficient funds" });
                return;
            }
            let newToBalance = user[toAccountType] + parseFloat(amount);

            await db_connect.collection("users").updateOne({ customerId }, { $set: { [fromAccountType]: newFromBalance, [toAccountType]: newToBalance } });

            const transaction = {
                customerId: customerId,
                type: "transfer",
                amount: parseFloat(amount),
                fromAccount: fromAccountType,
                toAccount: toAccountType,
                date: new Date(),
                description: `Transferred ${amount} from ${fromAccountType} to ${toAccountType}`,
            };
            await db_connect.collection("transactions").insertOne(transaction);
            res.json({ message: "Transfer successful", newFromBalance, newToBalance });
        } else {
            console.log("User not found:", customerId); // Debug log
            res.status(404).json({ message: "User not found" });
        }
    } catch (err) {
        console.error("Transfer route error:", err); // Debug log
        res.status(500).json({ error: err.message });
    }
});

// Fetch individual account transaction history
accountRoutes.route("/transactions/:customerId/:accountType").get(async (req, res) => {
    try {
        let db_connect = dbo.getDb();
        const transactions = await db_connect.collection("transactions").find({
            customerId: req.params.customerId,
            account: req.params.accountType
        }).toArray();
        res.json(transactions);
    } catch (err) {
        res.status(500).json({ error: err.message });
    }
});

// Fetch full transaction history
accountRoutes.route("/transactions/:customerId").get(async (req, res) => {
    try {
        let db_connect = dbo.getDb();
        const transactions = await db_connect.collection("transactions").find({ customerId: req.params.customerId }).toArray();
        res.json(transactions);
    } catch (err) {
        res.status(500).json({ error: err.message });
    }
});

// Fetch all accounts
accountRoutes.route("/accounts").get(async (req, res) => {
    try {
        let db_connect = dbo.getDb("bank_accounts");
        const result = await db_connect.collection("users").find({}).toArray();
        res.json(result);
    } catch (err) {
        throw err;
    }
});

// Fetch all customers
accountRoutes.route("/customers").get(async (req, res) => {
    try {
        let db_connect = dbo.getDb("bank_accounts");
        const result = await db_connect.collection("users").find({ "role": { $eq: "Customer" }}).toArray();
        res.json(result);
    } catch (err) {
        throw err;
    }
});

// Fetch single account by id
accountRoutes.route("/accounts/:id").get(async (req, res) => {
    try {
        let db_connect = dbo.getDb();
        let myquery = { _id: new ObjectId(req.params.id) };
        const result = await db_connect.collection("users").findOne(myquery);
        res.json(result);
    } catch (err) {
        res.status(500).json({ error: err.message });
    }
});

// Login route
accountRoutes.route("/accounts/login").post(async (req, res) => {
    try {
        let db_connect = dbo.getDb();
        let myobj = {
            firstname: req.body.firstname,
            lastname: req.body.lastname,
            email: req.body.email,
            phone: req.body.phone,
            password: req.body.password,
            savings: "",
            checking: "",
            roles: "",
        };
        const checkEmailAndPassword = await db_connect.collection("users").findOne({ email: myobj.email, password: myobj.password });
        if (!checkEmailAndPassword) {
            console.log("Error!!! Wrong!!");
            message = {message: "Error: No user"};
            res.json(message);
        } else {
            message = {message: "Success"};
            res.json(checkEmailAndPassword);
        }
    } catch (err) {
        throw err;
    }
});

// Update account by id
accountRoutes.route("/update/:id").put(async (req, res) => {
    try {
        let db_connect = dbo.getDb();
        let myquery = { _id: new ObjectId(req.params.id) };
        let newvalues = {
            $set: {
                firstname: req.body.firstname,
                lastname: req.body.lastname,
                email: req.body.email,
                phone: req.body.phone,
                password: req.body.password,
                role: req.body.role,
            },
        };
        const result = await db_connect.collection("users").updateOne(myquery, newvalues);
        res.json(result);
    } catch (err) {
        throw err;
    }
});

// Update balance by id
accountRoutes.route("/update-balance/:id").put(async (req, res) => {
    try {
        let db_connect = dbo.getDb();
        let myquery = { _id: new ObjectId(req.params.id) };
        let newvalues = {
            $set: {
                firstname: req.body.firstname,
                lastname: req.body.lastname,
                email: req.body.email,
                phone: req.body.phone,
                password: req.body.password,
                savings: req.body.savings,
                checking: req.body.checking,
                roles: "",
            },
        };
        const result = await db_connect.collection("users").updateOne(myquery, newvalues);
        res.json(result);
    } catch (err) {
        throw err;
    }
});

// Delete account by id
accountRoutes.route("/:id").delete(async (req, res) => {
    try {
        let db_connect = dbo.getDb();
        let myquery = { _id: new ObjectId(req.params.id) };
        const result = await db_connect.collection("users").deleteOne(myquery);
        res.json(result);
    } catch (err) {
        throw err;
    }
});

module.exports = accountRoutes;

