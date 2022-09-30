import React from "react";
import "./Legend.css";
import { BiError } from "react-icons/bi";
import { FaBomb, FaTools, FaRobot } from "react-icons/fa";
import { BsSuitHeartFill } from "react-icons/bs";
import { GiHeavyBullets, GiBrickWall, GiMineExplosion } from "react-icons/gi";
import { GoPrimitiveDot } from "react-icons/go";

export const Legend = () => {
  return (
    <div className="Legend_outer">
      <h1 className="Legend-text-wrapper">Legend</h1>
      <div className="Legend_inner">
        <FaRobot className="Legend_icon" />
        <h2 className="Legend_title">Player</h2>
      </div>
      <div className="Legend_inner">
        <FaBomb className="Legend_icon" />
        <h2 className="Legend_title">Bomb</h2>
      </div>
      <div className="Legend_inner">
        <FaTools className="Legend_icon" />
        <h2 className="Legend_title">Materials</h2>
      </div>
      <div className="Legend_inner">
        <BsSuitHeartFill className="Legend_icon" />
        <h2 className="Legend_title">Health</h2>
      </div>
      <div className="Legend_inner">
        <GiHeavyBullets className="Legend_icon" />
        <h2 className="Legend_title">Ammo</h2>
      </div>
      <div className="Legend_inner">
        <GiBrickWall className="Legend_icon" />
        <h2 className="Legend_title">Wall</h2>
      </div>
      <div className="Legend_inner">
        <GiMineExplosion className="Legend_icon" />
        <h2 className="Legend_title">Explosion</h2>
      </div>
      <div className="Legend_inner">
        <GoPrimitiveDot className="Legend_icon" />
        <h2 className="Legend_title">Bullet</h2>
      </div>
      <div className="Legend_inner">
        <BiError className="Legend_icon" />
        <h2 className="Legend_title">Error</h2>
      </div>
    </div>
  );
};
