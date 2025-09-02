import { GenrateId } from '../tools/tools.js';
export const login = (req, res) => {
    try {
        const { username, password } = req.body;
        if (!username || !password) {
            return res.status(401).send("Username or password is required");
        }

        // latter user id will be genrated only on registration and will be fetched from mongodb while login
        const id = GenrateId("Player", "User");
        return res.status(201).send(id);
    } catch (error) {
        console.log(`Error While Login Error: ${error}`);
        return res.status(500).json({ status: false, message: `Error Login and Error is :${error}` });
    }

}