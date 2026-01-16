// Firebase Helper Functions for Blazor Interop
import { initializeApp, getApps, getApp } from 'https://www.gstatic.com/firebasejs/12.7.0/firebase-app.js';

import { 
    getAuth,
    signInWithEmailAndPassword, 
    createUserWithEmailAndPassword, 
    signOut,
    onAuthStateChanged 
} from 'https://www.gstatic.com/firebasejs/12.7.0/firebase-auth.js';

import { 
    collection, 
    doc, 
    getDoc,
    getDocs,
    getFirestore, 
    onSnapshot, 
    setDoc, 
    addDoc, 
    updateDoc, 
    deleteDoc,
    query,
    where
} from 'https://www.gstatic.com/firebasejs/12.7.0/firebase-firestore.js';

// Initialize Firebase
const firebaseConfig = {
    apiKey: "AIzaSyDX_ypE-DlpBlpn-iSWOqre8Yn2v22_56Q",
    authDomain: "project5-arenanova.firebaseapp.com",
    databaseURL: "https://project5-arenanova-default-rtdb.firebaseio.com",
    projectId: "project5-arenanova",
    storageBucket: "project5-arenanova.firebasestorage.app",
    messagingSenderId: "119329144836",
    appId: "1:119329144836:web:748f91acd25ee91ec2b551",
    measurementId: "G-0D5RGT2P0H"
};

// Check if Firebase is already initialized to avoid duplicate app error
const app = !getApps().length ? initializeApp(firebaseConfig) : getApp();
const auth = getAuth(app);
const db = getFirestore(app);
const colref = collection(db, 'users');
const colref2 = collection(db, 'childProgress');
const colref3 = collection(db, 'childaccounts');

export async function firebaseSignIn(authParam, email, password) {
    try {
        const userCredential = await signInWithEmailAndPassword(auth, email, password);
        return {
            idToken: await userCredential.user.getIdToken(),
            refreshToken: userCredential.user.refreshToken,
            email: userCredential.user.email,
            localId: userCredential.user.uid,
            expiresIn: "3600"
        };
    } catch (error) {
        console.error("Firebase sign in error:", error);
        throw error;
    }
}

export async function firebaseSignUp(authParam, email, password) {
    try {
        const userCredential = await createUserWithEmailAndPassword(auth, email, password);
        const user = userCredential.user;
        
        return {
            idToken: await user.getIdToken(),
            refreshToken: user.refreshToken,
            email: user.email,
            localId: user.uid,
            expiresIn: "3600"
        };
    } catch (error) {
        console.error("Firebase sign up error:", error);
        throw error;
    }
}

export async function firebaseSignOut(authParam) {
    try {
        await signOut(auth);
        return { success: true };
    } catch (error) {
        console.error("Firebase sign out error:", error);
        return { success: false, error: error.message };
    }
}

export function firebaseOnAuthStateChanged(authParam, dotNetHelper) {
    onAuthStateChanged(auth, user => {
        if (user) {
            dotNetHelper.invokeMethodAsync('OnAuthStateChanged', user.uid);
        } else {
            dotNetHelper.invokeMethodAsync('OnAuthStateChanged', null);
        }
    });
}

export async function getUserData(uid) {
    const docRef = doc(colref, uid);
    const docSnap = await getDoc(docRef);
    if (docSnap.exists()) {
        return docSnap.data();
    } else {
        return null;
    }
}

export async function getUserAndChildProgress(userUid, childUid) {
    const userRef = doc(colref, userUid);
    const childRef = doc(colref2, childUid);

    const userSnap = await getDoc(userRef);
    const childSnap = await getDoc(childRef);

    return {
        user: userSnap.exists() ? userSnap.data() : null,
        childProgress: childSnap.exists() ? childSnap.data() : null
    };
}

export async function firebasestoreAddChildAccount(data) {
    return await addDoc(colref3, data);
}

export async function firebaseSetDocument(collection, documentId, data) {
    try {
        await setDoc(doc(db, collection, documentId), data);
    } catch (error) {
        console.error("Firebase set document error:", error);
        throw error;
    }
}

export async function firebaseGetDocument(collectionName, documentId) {
    try {
        // Check if user is authenticated
        const currentUser = auth.currentUser;
        console.log('firebaseGetDocument - Current user:', currentUser ? currentUser.uid : 'NOT AUTHENTICATED');
        console.log('firebaseGetDocument - Attempting to get:', `${collectionName}/${documentId}`);
        
        if (!currentUser) {
            console.error('User is not authenticated! Cannot read from Firestore.');
            throw new Error('User must be authenticated to read from Firestore');
        }
        
        const docRef = doc(db, collectionName, documentId);
        const docSnap = await getDoc(docRef);
        if (docSnap.exists()) {
            console.log(`Document ${collectionName}/${documentId} loaded successfully`);
            return { id: docSnap.id, ...docSnap.data() };
        } else {
            console.log(`Document ${collectionName}/${documentId} does not exist`);
            return null;
        }
    } catch (error) {
        console.error(`Firebase get document error (${collectionName}/${documentId}):`, error);
        throw error;
    }
}

export async function firebaseGetCollection(collectionName) {
    try {
        const snapshot = await getDocs(collection(db, collectionName));
        const results = [];
        snapshot.forEach((doc) => {
            results.push({ id: doc.id, ...doc.data() });
        });
        return results;
    } catch (error) {
        console.error("Firebase get collection error:", error);
        throw error;
    }
}

export async function firebaseGetSubcollection(parentCollection, docId, subcollectionName) {
    try {
        const subcolRef = collection(db, parentCollection, docId, subcollectionName);
        const snapshot = await getDocs(subcolRef);
        const results = [];
        snapshot.forEach((doc) => {
            const data = { id: doc.id, ...doc.data() };
            results.push(data);
            // Log first document's raw data to see actual field names
            if (results.length === 1) {
                console.log(`First document in ${parentCollection}/${docId}/${subcollectionName}:`, data);
                console.log('Field names:', Object.keys(data));
            }
        });
        console.log(`Loaded ${results.length} documents from ${parentCollection}/${docId}/${subcollectionName}`);
        return results;
    } catch (error) {
        console.error(`Firebase get subcollection error (${parentCollection}/${docId}/${subcollectionName}):`, error);
        throw error;
    }
}

export async function firebasestoreUpdateUserData(uid, data) {
    const docRef = doc(colref, uid);
    return await updateDoc(docRef, data);
}
export async function firebasestoreUpdateChildProgress(childUid, data) {
    const docRef = doc(colref2, childUid);
    return await updateDoc(docRef, data);
}

export async function firebasestoreDeleteChildAccount(childUid) {
    const docRef = doc(colref3, childUid);
    return await deleteDoc(docRef);
}

export function firebasestoreOnChildAccountsChanged(userUid, dotNetHelper) {
    const q = query(colref3, where('parentUid', '==', userUid));
    return onSnapshot(q, (querySnapshot) => {
        const childAccounts = [];
        querySnapshot.forEach((doc) => {
            childAccounts.push({ id: doc.id, ...doc.data() });
        }
        );
        dotNetHelper.invokeMethodAsync('OnChildAccountsChanged', childAccounts);
    });
}
export function firebasestoreOnChildProgressChanged(childUid, dotNetHelper) {
    const docRef = doc(colref2, childUid);
    return onSnapshot(docRef, (docSnapshot) => {
        if (docSnapshot.exists()) {
            dotNetHelper.invokeMethodAsync('OnChildProgressChanged', docSnapshot.data());
        }
    });
}

export async function countsubcollections(docPath) {
    const docRef = doc(db, docPath);
    const subcollections = await getSubcollections(docRef);
    return subcollections.length;
}

export async function listSubcollectionNames(docPath) {
    const docRef = doc(db, docPath);
    const subcollections = await getSubcollections(docRef);
    return subcollections.map(subcol => subcol.id);
}
async function getSubcollections(docRef) {
    return await getCollections(docRef);
}

export async function storeuidinlocalstorage(uid) {
    localStorage.setItem('firebaseUid', uid);
}

export async function getuidfromlocalstorage() {
    return localStorage.getItem('firebaseUid');
}
export async function removeuidfromlocalstorage() {
    localStorage.removeItem('firebaseUid');
}
export async function clearlocalstorage() {
    localStorage.clear();
}

export async function storechilduidinlocalstorage(childUid) {
    localStorage.setItem('childUid', childUid);
}
export async function getchilduidfromlocalstorage() {
    return localStorage.getItem('childUid');
}
export async function removechilduidfromlocalstorage() {
    localStorage.removeItem('childUid');
}
export async function querySnapshot() {
    const qUsers = query(colref);
    const qChildrenProgress = query(colref2);
    const qChildAccounts = query(colref3);

    const usersSnap = await getDocs(qUsers);
    const childrenProgressSnap = await getDocs(qChildrenProgress);
    const childAccountsSnap = await getDocs(qChildAccounts);

    const users = [];
    usersSnap.forEach((doc) => {
        users.push({ id: doc.id, ...doc.data() });
    });

    const childrenProgress = [];
    childrenProgressSnap.forEach((doc) => {
        childrenProgress.push({ id: doc.id, ...doc.data() });
    });

    const childAccounts = [];
    childAccountsSnap.forEach((doc) => {
        childAccounts.push({ id: doc.id, ...doc.data() });
    });

    return {
        users,
        childrenProgress,
        childAccounts
    };
}

// Get all documents from a subcollection under a specific document
export async function getSubcollection(parentCollection, docId, subcollectionName) {
    const subcolRef = collection(db, parentCollection, docId, subcollectionName);
    const snapshot = await getDocs(subcolRef);
    const results = [];
    snapshot.forEach((doc) => {
        results.push({ id: doc.id, ...doc.data() });
    });
    return results;
}
export function getCurrentUid(authParam) {
    return auth.currentUser ? auth.currentUser.uid : null;
}

export function getcertaindata(subcollectionName, fieldName, value) {
    const q = query(collection(db, subcollectionName), where(fieldName, "==", value));
    return getDocs(q).then((querySnapshot) => {
        const results = [];
        querySnapshot.forEach((doc) => {
            results.push({ id: doc.id, ...doc.data() });
        });
        return results;
    }).catch((error) => {
        console.error("Error getting documents: ", error);
        throw error;
    });
}