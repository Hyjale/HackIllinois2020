from flask import Flask, render_template
import os

app = Flask(__name__)

@app.route("/")
def home():
    users = os.listdir('static/images')
    interactions = ['gaze', 'point', 'grab', 'generic']
    graphs = {}

    for user in users:
        for interaction in interactions:
            imagePath = 'static/images/' + user + '/' + interaction
            fileList = os.listdir(imagePath)
            images = [imagePath[7:] + '/' + file for file in fileList]
            
            pair = [user, images]

            if interaction not in graphs:
                graphs[interaction] = []
            graphs[interaction].append(pair)

    #images = ['images/' + file for file in images]
    return render_template("dashboard.html", users = users, graphs=graphs)

if __name__ == "__main__":
    app.run(debug=True)
